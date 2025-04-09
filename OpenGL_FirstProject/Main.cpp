#include <iostream>
#include <fstream>
#include <sstream>
#include <string>
#include <vector>
#include <Windows.h>
#include <glad\glad.h>
#include <GLFW\glfw3.h>
#include <glm\glm.hpp>
#include <glm\gtx\transform.hpp>
#include <glm\gtc\type_ptr.hpp>
#include "Figure.h"

std::vector<model> models;
bool mouse_action = false;
double lastX, lastY;// последняя позиция курсора
glm::mat4 T; // матрица, в которой накапливаются все преобразования
glm::vec3 S, P, u; // координаты точки наблюдения.  Тчк, в которую направлен вектор наблюдения. 
//Вектора направления вверх
float dist; //расстояние между S и P
float fovy, aspect; // угол обзора и соотношение сторон окна наблюдения
float fovy_work, aspect_work; //рабочие переменные
float near_view, far_view; // расстояния до окна наблюдения и до горизонта
float n, f; // рабочие переменные для near и far
float l, r, t, b; //рабочие переменные для знач. коорд. левой, правой, нижней и верхней коорд. в СКН
enum projType { Ortho, Frustum, Perspective } pType; // тип трехмерной проекции

void initWorkPars() {// инициализация рабочих параметров камеры
	n = near_view;
	f = far_view;
	fovy_work = fovy;
	aspect_work = aspect;
	float Vy = 2 * near_view * glm::tan(fovy / 2);
	float Vx = aspect * Vy;
	l = -Vx / 2;
	r = Vx / 2;
	b = -Vy / 2;
	t = Vy / 2;
	dist = glm::length(P - S);
	T = glm::lookAt(S, P, u);
}

void readFromFile(const char* fileName) {// чтение сцены из файла fileName
	std::ifstream in;
	in.open(fileName);
	if (in.is_open()) {
		// файл успешно открыт
		models.clear(); // очищаем имеющийся список ломаных
		// временные переменные для чтения из файла
		glm::mat4 M = glm::mat4(1.f); // матрица для получения модельной матрицы
		glm::mat4 initM; // матрица для начального преобразования каждого рисункa
		std::vector<glm::mat4> transforms; // стек матриц преобразований
		std::vector<path> figure;  // список ломаных очередного рисунка
		float thickness = 2; // толщина со значением по умолчанию 2
		float r, g, b; // составляющие цвета
		r = g = b = 0; // (черный)
		std::string cmd; // строка для считывания имени команды
		// непосредственно работа с файлом
		std::string str;// строка, в которую считываем строки файла
		std::getline(in, str);// считываем из входного файла первую строку
		while (in) { // если очередная строка считана успешно
			// обрабатываем строку
			if ((str.find_first_not_of(" \t\r\n") != std::string::npos) && (str[0] != '#')) {
				// прочитанная строка не пуста и не комментарий
				std::stringstream s(str); // строковый поток из строки str
				s >> cmd;
				if (cmd == "camera") { // положение камеры
					float x, y, z; // промежуточные переменные для чтения из файла, чтобы не упал → Intellisense
					s >> x >> y >> z;// координаты точки наблюдения
					S = glm::vec3(x, y, z);
					s >> x >> y >> z;// точка, в которую направлен вектор наблюдения
					P = glm::vec3(x, y, z);
					s >> x >> y >> z;; // вектор направления вверх
					u = glm::vec3(x, y, z);
				}
				else if (cmd == "screen") { // положение окна наблюдения
					s >> fovy_work >> aspect >> near_view >> far_view;
					fovy = glm::radians(fovy_work);
				}
				else if (cmd == "color") {// цвет линии
					s >> r >> g >> b;
				}
				else if (cmd == "thickness") { // толщина линии
					s >> thickness;
				}
				else if (cmd == "path") { // набор точек
					std::vector<glm::vec3> vertices;  // список точек ломаной
					int N; // количество точек
					s >> N;
					std::string str1; // дополнительная строка для чтения из файла
					while (N > 0) { // пока не все точки считали
						std::getline(in, str1); // считываем в str1 из входного файла очередную строку
						// так как файл корректный, то на конец файла проверять не нужно
						if ((str1.find_first_not_of(" \t\r\n") != std::string::npos) && (str1[0] != '#')) {
							// прочитанная строка не пуста и не комментарий значит в ней пара координат
							float x, y, z; // переменные для считывания
							std::stringstream s1(str1); // еще один строковый поток из строки str1
							s1 >> x >> y >> z;
							vertices.push_back(glm::vec3(x, y, z)); // добавляем точку в список
							N--; // уменьшаем счетчик после успешного считывания точки
						}
					}
					// все точки считаны, генерируем ломаную (path) и кладем ее в список figure
					figure.push_back(path(vertices, glm::vec3(r, g, b) / 255.f, thickness));
				}
				else if (cmd == "model") { // начало описания нового рисунка
					float mVcx, mVcy, mVcz, mVx, mVy, mVz; // параметры команды model
					s >> mVcx >> mVcy >> mVcz >> mVx >> mVy >> mVz; // считываем значения переменных
					float S = mVx / mVy < 1 ? 2.f / mVy : 2.f / mVx;
					// сдвиг точки привязки из начала координат в нужную позицию
					// после которого проводим масштабирование
					initM = glm::scale(glm::vec3(S)) * glm::translate(glm::vec3(-mVcx, -mVcy, -mVcz));
					figure.clear();
				}
				else if (cmd == "figure") { // формирование новой модели
					models.push_back(model(figure, M * initM)); // добавляем рисунок в список
				}
				else if (cmd == "translate") { // перенос
					float Tx, Ty, Tz;  // параметры преобразования переноса
					s >> Tx >> Ty >> Tz; // считываем параметры
					M = glm::translate(glm::vec3(Tx, Ty, Tz)) * M; // добавляем перенос к общему преобразованию
				}
				else if (cmd == "scale") { // масштабирование
					float S; // параметр масштабирования
					s >> S;  // считываем параметр
					M = glm::scale(glm::vec3(S)) * M; // добавляем масштабирование к общему преобразованию
				}
				else if (cmd == "rotate") { // поворот
					float theta; // угол поворота в градусах
					float nx, ny, nz; // координаты направляющего вектора оси вращения
					s >> theta >> nx >> ny >> nz;
					M = glm::rotate(glm::radians(theta), glm::vec3(nx, ny, nz)) * M;// добавляем поворот к общему преобразованию
				}
				else if (cmd == "pushTransform") { // сохранение матрицы в стек
					transforms.push_back(M); // сохраняем матрицу в стек
				}
				else if (cmd == "popTransform") { // откат к матрице из стека
					M = transforms.back(); // получаем верхний элемент стека
					transforms.pop_back(); // выкидываем матрицу из стека
				}
			}
			// считываем очередную строку
			std::getline(in, str);
		}
		initWorkPars();
	}
}

// обработчик события Resize
void framebuffer_size_callback(GLFWwindow* window, int width, int height)
{
	glViewport(0, 0, width, height);
}

// матрица вращения относительно оси, заданной вектором n,
// проходящей через точку P
glm::mat4 rotateP(float theta, glm::vec3 n, glm::vec3 P) {
	return glm::translate(P) * glm::rotate(theta, n) * glm::translate(-P);
}

// обработчик положения курсора
void cursorPos_callback(GLFWwindow* window, double xpos, double ypos) {
	// вычисляем вектор, задающий ось вращения
	glm::vec3 n = glm::vec3(lastY - ypos, lastX - xpos, 0);
	// создаем матрицу вращения
	glm::mat4 M = glm::rotate(glm::length(n) * 0.002f, n);
	// вращаем току (0, 0, -1), на которую смотрит наблюдатель
	glm::vec3 P = M * glm::vec4(0, 0, -1, 1);
	// добавляем к преобразованиям переход к новой системе координат наблюдателя
	T = glm::lookAt(glm::vec3(0), P, glm::vec3(0, 1, 0)) * T;
	lastX = xpos;
	lastY = ypos;
}

// обработчик позиции курсора при включенном курсоре
void cursorPosSave_callback(GLFWwindow* window, double xpos, double ypos) {
	if (mouse_action) {
		glm::vec3 n = glm::vec3(lastY - ypos, lastX - xpos, 0);
		glm::mat4 M = rotateP(0.1, n, glm::vec3(0, 0, -dist));
		glm::vec3 u_new = glm::mat3(M) * glm::vec3(0, 1, 0);
		glm::vec3 S_new = glm::vec3(M * glm::vec4(0, 0, 0, 1));
		glm::vec3 P = M * glm::vec4(0, 0, -1, 1);
		T = lookAt(S_new, P, u_new) * T;
	}
	lastX = xpos;
	lastY = ypos;
}

// обработчик панелей прокрутки
void scroll_callback(GLFWwindow* window, double xoffset, double yoffset) {
	dist = yoffset;
}

// обработчик нажатия кнопки мыши
void mouse_button_callback(GLFWwindow* window, int button, int action, int mods) {
	if (glfwGetInputMode(window, GLFW_CURSOR) != GLFW_CURSOR_DISABLED && button == GLFW_MOUSE_BUTTON_LEFT) {
		if (action == GLFW_PRESS)
			mouse_action = true;
		else if (action == GLFW_RELEASE)
			mouse_action = false;
	}
}


// Обработчик нажатия клавиш
void key_callback(GLFWwindow* window, int key, int scancode, int action, int mode)
{
	if (action != GLFW_RELEASE) // если клавиша нажата
	{
		switch (key)// анализируем обрабатываемую клавишу
		{
		case GLFW_KEY_ESCAPE:
			initWorkPars();
			break;
		case GLFW_KEY_1:
			pType = Ortho;
			break;
		case GLFW_KEY_2:
			pType = Frustum;
			break;
		case GLFW_KEY_3:
			pType = Perspective;
			break;
		case GLFW_KEY_F3: {
			OPENFILENAME openFileDialog; // диалог открытия файла
			char fileName[260];// буфер для имени файла
			// Инициализация файлового диалога
			ZeroMemory(&openFileDialog, sizeof(openFileDialog));
			openFileDialog.lStructSize = sizeof(openFileDialog);
			openFileDialog.hwndOwner = NULL;
			openFileDialog.lpstrFile = fileName;
			openFileDialog.lpstrFile[0] = '\0';
			openFileDialog.nMaxFile = sizeof(fileName);
			openFileDialog.lpstrFilter = "Text files (*.txt)\0*.txt\0All files 2007\0*.*\0";
			openFileDialog.nFilterIndex = 1;
			openFileDialog.lpstrFileTitle = NULL;
			openFileDialog.nMaxFileTitle = 0;
			openFileDialog.lpstrInitialDir = NULL;
			openFileDialog.Flags = OFN_PATHMUSTEXIST | OFN_FILEMUSTEXIST | OFN_HIDEREADONLY;
			// Вывод файлового диалога и получение результата
			if (GetOpenFileName(&openFileDialog)) {
				// если файл выбран успешно
				readFromFile(fileName);
			}
		}
		case GLFW_KEY_F5:
			if (glfwGetInputMode(window, GLFW_CURSOR) == GLFW_CURSOR_DISABLED) {
				// включается курсор
				glfwSetInputMode(window, GLFW_CURSOR, GLFW_CURSOR_NORMAL);
				// отключается обработчик положения курсора
				glfwSetCursorPosCallback(window, cursorPosSave_callback);

			}
			else {
				// отключается курсор
				glfwSetInputMode(window, GLFW_CURSOR, GLFW_CURSOR_DISABLED);
				// включается обработчик положения курсора
				glfwSetCursorPosCallback(window, cursorPos_callback);
			}
			break;
		case GLFW_KEY_W: {
			if (mode == GLFW_MOD_SHIFT) {
				T = glm::lookAt(glm::vec3(0, 0, -0.1), glm::vec3(0, 0, -0.2), glm::vec3(0, 0.1, 0)) * T;
			}
			else {
				T = lookAt(glm::vec3(0, 0, -1), glm::vec3(0, 0, -2), glm::vec3(0, 1, 0)) * T;
			}
			break;
		}
		case GLFW_KEY_S: {
			if (mode == GLFW_MOD_SHIFT) {
				T = glm::lookAt(glm::vec3(0, 0, 0.1), glm::vec3(0, 0, 0), glm::vec3(0, 0.1, 0)) * T;
			}
			else {
				T = glm::lookAt(glm::vec3(0, 0, 1), glm::vec3(0, 0, 0), glm::vec3(0, 1, 0)) * T;
			}
			break;
		}
		case GLFW_KEY_A: {
			if (mode == GLFW_MOD_SHIFT) {
				T = glm::lookAt(glm::vec3(-0.1, 0, 0), glm::vec3(-0.1, 0, -0.1), glm::vec3(0, 1, 0)) * T;
			}
			else {
				T = glm::lookAt(glm::vec3(-1, 0, 0), glm::vec3(-1, 0, -1), glm::vec3(0, 1, 0)) * T;
			}
			break;
		}
		case GLFW_KEY_D: {
			if (mode == GLFW_MOD_SHIFT) {
				T = glm::lookAt(glm::vec3(0.1, 0, 0), glm::vec3(0.1, 0, -0.1), glm::vec3(0, 1, 0)) * T;
			}
			else {
				T = glm::lookAt(glm::vec3(1, 0, 0), glm::vec3(1, 0, -1), glm::vec3(0, 1, 0)) * T;
			}
			break;
		}
		case GLFW_KEY_R: {////поворот наблюдателя отн OZ по часовой на угол 0.1 рад
			glm::vec3 u_new = glm::mat3(glm::rotate(0.1f, glm::vec3(0, 0, 1))) * glm::vec3(0, 1, 0);
			T = glm::lookAt(glm::vec3(0, 0, 0), glm::vec3(0, 0, -1), u_new) * T;
			break;
		}
		case GLFW_KEY_Y: {//поворот камеры отн OZ по часовой на угол 0.1 рад
			glm::vec3 u_new = glm::mat3(glm::rotate(0.1f, glm::vec3(0, 0, -1))) * glm::vec3(0, 1, 0);
			T = glm::lookAt(glm::vec3(0, 0, 0), glm::vec3(0, 0, -1), u_new) * T;
			break;
		}
		case GLFW_KEY_T:
		{
			//P — точки, отстоящей от начала координат в отрицательном направлении оси Oz на расстояние dist.
			if (mode == GLFW_MOD_SHIFT) {
				glm::mat4 M = rotateP(0.1, glm::vec3(1, 0, 0), glm::vec3(0, 0, -dist));// матрица вращения относительно точки P
				glm::vec3 u_new = glm::mat3(M) * glm::vec3(0, 1, 0); // вращение направления вверх
				glm::vec3 S_new = glm::vec3(M * glm::vec4(0, 0, 0, 1)); // вращение начала координат
				// переход к СКН в которой начало координат в новой точке, а направление
				// наблюдения - в точку P
				T = glm::lookAt(S_new, glm::vec3(0, 0, -dist), u_new) * T;
			}
			else {
				glm::mat4 M = glm::rotate(0.1f, glm::vec3(1, 0, 0)); // матрица вращения относительно Ox
				glm::vec3 u_new = glm::mat3(M) * glm::vec3(0, 1, 0); // вращение направления вверх
				glm::vec3 P_new = glm::vec3(M * glm::vec4(0, 0, -1, 1)); // вращение точки, в которую смотрит наблюдатель
				T = glm::lookAt(glm::vec3(0, 0, 0), P_new, u_new) * T;
			}
			break;
		}
		case GLFW_KEY_G: {
			if (mode == GLFW_MOD_SHIFT) {
				//разворот камеры на 0.1 рад по часовой стрелке относительно оси,
				//параллельной Ox и проходящей через начало координат
				glm::mat4 M = rotateP(-0.1, glm::vec3(-1, 0, 0), glm::vec3(0, 0, -dist));
				glm::vec3 u_new = glm::mat3(M) * glm::vec3(0, 1, 0);
				glm::vec3 S_new = glm::vec3(M * glm::vec4(0, 0, 0, -1));


				T = glm::lookAt(S_new, glm::vec3(0, 0, -dist), u_new) * T;
			}
			else {//разворот камеры на 0.1 рад по часовой стрелке относительно оси, 
				//параллельной Ox и проходящей через условную точку
				glm::mat4 M = rotate(-0.1f, glm::vec3(-1, 0, 0));
				glm::vec3 u_new = glm::mat3(M) * glm::vec3(0, 1, 0);
				glm::vec3 P_new = glm::vec3(M * glm::vec4(0, 0, 1, -1));
				T = glm::lookAt(glm::vec3(0, 0, 0), P_new, u_new) * T;
			}
			break;
		}
		case GLFW_KEY_F: {
			if (mode == GLFW_MOD_SHIFT) {
				//разворот камеры на 0.1 рад по часовой отн оси,
				//пралл ОУ и проходящей через нач коорд
				glm::mat4 M = rotateP(0.1, glm::vec3(0, 1, 0), glm::vec3(0, 0, -dist));
				glm::vec3 u_new = glm::mat3(M) * glm::vec3(0, 1, 0);
				glm::vec3 S_new = glm::vec3(M * glm::vec4(0, 0, 0, 1));
				T = glm::lookAt(S_new, glm::vec3(0, 0, -dist), u_new) * T;
			}
			else {//через Р
				glm::mat4 M = rotate(0.1f, glm::vec3(0, 1, 0));
				glm::vec3 u_new = glm::mat3(M) * glm::vec3(0, 1, 0);
				glm::vec3 P_new = glm::vec3(M * glm::vec4(0, 0, -1, 1));
				T = glm::lookAt(glm::vec3(0, 0, 0), P_new, u_new) * T;
			}
			break;
		}
		case GLFW_KEY_H: {//разворот камеры на 0.1 рад против часовой отн оси,
			//пралл ОУ и проходящей через нач коорд
			if (mode == GLFW_MOD_SHIFT) {
				//разворот камеры на 0.1 рад против часовой отн оси,
				//пралл ОУ и проходящей через нач коорд
				glm::mat4 M = rotateP(-0.1, glm::vec3(0, 1, 0), glm::vec3(0, 0, -dist));
				glm::vec3 u_new = glm::mat3(M) * glm::vec3(0, 1, 0);
				glm::vec3 S_new = glm::vec3(M * glm::vec4(0, 0, 0, 1));
				T = glm::lookAt(S_new, glm::vec3(0, 0, -dist), u_new) * T;
			}
			else {//через Р
				glm::mat4 M = rotate(-0.1f, glm::vec3(0, 1, 0));
				glm::vec3 u_new = glm::mat3(M) * glm::vec3(0, 1, 0);
				glm::vec3 P_new = glm::vec3(M * glm::vec4(0, 0, -1, 1));
				T = glm::lookAt(glm::vec3(0, 0, 0), P_new, u_new) * T;
			}
			break;
		}
		case GLFW_KEY_I:
			if (mode == GLFW_MOD_SHIFT) {
				//уменьшение рабочего знач t на 1 ед СКН
				t -= 1;
			}
			else { //увеличение -/-
				t += 1;
			}
			break;
		case GLFW_KEY_J: //тоже самое с l
			if (mode == GLFW_MOD_SHIFT) {
				l += 1;
			}
			else {
				l -= 1;
			}
			break;
		case GLFW_KEY_K: {//уменьшение/увеличение окна наблюдателя, изменяя b 
			if (mode == GLFW_MOD_SHIFT) {
				b -= 1;
			}
			else {
				b += 1;
			}
			break;
		}
		case GLFW_KEY_L: {//тоже самое с r
			if (mode == GLFW_MOD_SHIFT) {
				r -= 1;
			}
			else {
				r += 1;
			}
			break;
		}
		case GLFW_KEY_U: {//увеличение уменьшение параметра n на 0.2, ограничив его снизу
			//значением 0.1 и сверху значением(f − 0.1)
			if (mode == GLFW_MOD_SHIFT) {
				if (n >= 0.1) {
					n -= 0.2;
				}
				else break;
			}
			else {
				if (n <= f - 0.1) {
					n += 0.2;
				}
				else break;
			}
			break;
		}
		case GLFW_KEY_O: {//увеличение уменьшение параметра f на 0.2, 
			//ограничив его снизу значением(n + 0.1)
			if (mode == GLFW_MOD_SHIFT) {
				if (f >= n + 0.1) {
					f -= 0.2;
				}
				else break;
			}
			else {
				f += 0.2;
			}
			break;
		}
		case GLFW_KEY_B: {//увеличение уменьшение параметра dist на 0.2, 
			//ограничив его снизу значением 0.1;
			if (mode == GLFW_MOD_SHIFT) {
				if (dist >= 0.1) {
					dist -= 0.2;
				}
				else break;
			}
			else {
				dist += 0.2;
			}
			break;
		}
		case GLFW_KEY_Z: {//увеличение уменьшение параметра fovy_work, 
			//ограничив его значения диапазоном от 0.3 до 3 радиан;

			if (mode == GLFW_MOD_SHIFT) {
				if (fovy_work >= 0.3) {
					fovy_work -= 0.05;
				}
				else break;
			}
			else {
				if (fovy_work <= 3) {
					fovy_work += 0.05;
				}
				else break;
			}
			break;
		}
		case GLFW_KEY_X: {//увеличение уменьшение параметра aspect_work на 0.05, 
			//ограничив его снизу значением 0.01;
			if (mode == GLFW_MOD_SHIFT) {
				if (aspect_work >= 0.01) {
					aspect_work -= 0.05;
				}
				else break;
			}
			else {
				aspect_work += 0.05;
			}
			break;
		}

		default:
			break;
		}
	}
}

int main() {
	glfwInit(); // Инициализация GLFW

	// Задается минимальная требуемая версия OpenGL.
	glfwWindowHint(GLFW_CONTEXT_VERSION_MAJOR, 3); // Номер до десятичной точки
	glfwWindowHint(GLFW_CONTEXT_VERSION_MINOR, 3); // Номер после десятичной точки
	// Используем только средства указанной версии без совместимости с более ранними
	glfwWindowHint(GLFW_OPENGL_PROFILE, GLFW_OPENGL_CORE_PROFILE);

	// Создаем окно для вывода графики
	GLFWwindow* window = glfwCreateWindow(800, 600, "task_7_Vasilenko", NULL, NULL);
	if (window == NULL) {// если ссылка на окно не создана
		std::cout << "The glfwCreateWindow call failed." << std::endl;
		glfwTerminate(); // завершить работу GLFW
		return -1;       // завершить программу
	}
	glfwMakeContextCurrent(window); // делаем окно window активным (текущим)
	// Назначение обработчика события Resize
	glfwSetFramebufferSizeCallback(window, framebuffer_size_callback);
	// Назначение обработчика нажатия клавиш
	glfwSetKeyCallback(window, key_callback);
	// назначение обработчика положения курсора
	glfwSetCursorPosCallback(window, cursorPosSave_callback);

	glfwSetScrollCallback(window, scroll_callback);

	glfwSetMouseButtonCallback(window, mouse_button_callback);

	// Инициализация GLAD
	if (!gladLoadGLLoader((GLADloadproc)glfwGetProcAddress)) {
		std::cout << "Failed to load GLAD" << std::endl;
		glfwTerminate();// завершить работу GLFW
		return -1;      // завершить программу
	}

	// сообщаем диапазон координат в окне
	// (0,0) - координаты левого нижнего угла, 800x600 - размеры окна в пикселах
	glViewport(0, 0, 800, 600);

	//ВЕРШИННЫЙ ШЕЙДЕР
	const char* vertexShaderSource =
		"#version 460 core\n"
		"layout (location = 0) in vec3 position;\n"
		"uniform mat4 clipView;\n"
		"void main() {\n"
		"   gl_Position = clipView * vec4(position, 1.0);\n"
		"}\0";
	GLuint vertexShader; // шейдерный объект - вершинный шейдер
	vertexShader = glCreateShader(GL_VERTEX_SHADER); // создаем объект
	// привязываем исходный код к шейдерному объекту
	glShaderSource(vertexShader, 1, &vertexShaderSource, NULL);
	glCompileShader(vertexShader); // компилируем шейдер

	//ПРОВЕРКА, ЧТО КОМПИЛЯЦИЯ УСПЕШНА
	GLint success;// результат компиляции
	// запрашиваем статус компиляции шейдера в переменную success
	glGetShaderiv(vertexShader, GL_COMPILE_STATUS, &success);
	if (!success) {          // если компиляция прошла с ошибкой
		GLchar infoLog[512];// объявим контейнер для сообщения об ошибке
		glGetShaderInfoLog(vertexShader, 512, NULL, infoLog);// запрашиваем сообщение
		std::cout << "ERROR::SHADER::VERTEX::COMPILATION_FAILED\n" << infoLog << std::endl;// выводим сообщение об ошибке на экран
	}

	//ФРАГМЕНТНЫЙ ШЕЙДЕР
	const char* fragmentShaderSource =
		"#version 460 core\n"
		"out vec4 color;\n"
		"uniform vec3 pathColor;\n"
		"void main(){\n"
		"   color = vec4(pathColor, 1.0f);\n"
		"}\n\0";
	GLuint fragmentShader; // шейдерный объект - фрагментный шейдер
	fragmentShader = glCreateShader(GL_FRAGMENT_SHADER); // создаем объект
	// привязываем исходный код к шейдерному объекту
	glShaderSource(fragmentShader, 1, &fragmentShaderSource, NULL);
	glCompileShader(fragmentShader); // компилируем шейдер

	//ПРОВЕРКА, ЧТО КОМПИЛЯЦИЯ УСПЕШНА
	// запрашиваем статус компиляции шейдера в переменную success
	glGetShaderiv(fragmentShader, GL_COMPILE_STATUS, &success);
	if (!success) {          // если компиляция прошла с ошибкой
		GLchar infoLog[512];// объявим контейнер для сообщения об ошибке
		glGetShaderInfoLog(fragmentShader, 512, NULL, infoLog);// запрашиваем сообщение
		std::cout << "ERROR::SHADER::FRAGMENT::COMPILATION_FAILED\n" << infoLog << std::endl;// выводим сообщение об ошибке на экран
	}

	//СБОРКА ШЕЙДЕРНОЙ ПРОГРАФММЫ
	GLuint shaderProgram;// идентификатор шейдерной программы
	shaderProgram = glCreateProgram();// создаем программный объект
	glAttachShader(shaderProgram, vertexShader);//подключение вершиного
	glAttachShader(shaderProgram, fragmentShader); //фрагментного
	glLinkProgram(shaderProgram);// компоновка программы
	// запрашиваем статус компоновки шейдерной программы в переменную success
	glGetProgramiv(shaderProgram, GL_TRIANGLES, &success);
	if (!success) {          // если компоновка прошла с ошибкой
		GLchar infoLog[512];// объявим контейнер для сообщения об ошибке
		glGetShaderInfoLog(shaderProgram, 512, NULL, infoLog);// запрашиваем сообщение
		std::cout << "ERROR::SHADER::PROGRAM::COMPILATION_FAILED\n" << infoLog << std::endl;// выводим сообщение об ошибке на экран
	}

	// удаление шейдерных объектов
	glDeleteShader(vertexShader);
	glDeleteShader(fragmentShader);

	// запрашиваем у программы shaderProgram расположение переменной pathColor
	GLint pathColorLocation = glGetUniformLocation(shaderProgram, "pathColor");
	// запрашиваем у программы shaderProgram расположение переменной clipView
	GLint clipViewLocation = glGetUniformLocation(shaderProgram, "clipView");

	readFromFile("triangle.txt");

	/*// НАБОР ИСХОДНЫХ ДАННЫХ ДЛЯ ОТРИСОВКИ
	GLfloat vertices[] = {
		-0.5f, -0.5f, 0.0f,
		0.5f, -0.5f, 0.0f,
		0.0f, 0.5f, 0.0f
	};

	GLuint vertexArray;// объект вершинного массива
	// создаем вершинный массив, идентификатор которого присваиваем vertexArray
	glGenVertexArrays(1, &vertexArray);
	glBindVertexArray(vertexArray);// делаем активным вершинный массив для накопления информации в вершином массиве

	GLuint vertexBufffer;// идентификатор буферного объекта
	// создаем буферный объект, идентификатор которого присваиваем vertexBuffer
	glGenBuffers(1, &vertexBufffer);

	// привязка vertexBuffer к GL_ARRAY_BUFFER
	glBindBuffer(GL_ARRAY_BUFFER, vertexBufffer);
	// в буфер, привязанный к GL_ARRAY_BUFFER копируем содержимое vertices
	glBufferData(GL_ARRAY_BUFFER, sizeof(vertices), vertices, GL_STATIC_DRAW);

	// описание расположения параметра вершинного шейдера в вершинном буфере
	glVertexAttribPointer(0, 3, GL_FLOAT, GL_FALSE, 3 * sizeof(GLfloat), (GLvoid*)0);
	glEnableVertexAttribArray(0);// включение параметра 0 для шейдера
	glBindBuffer(GL_ARRAY_BUFFER, 0);// отвязка буферного объекта
	glBindVertexArray(0);// отключение вершинного массива

	//ОТРИСОВКА ПРИМИТИВОВ
	glUseProgram(shaderProgram);// шейдерную программу shaderProgram делаем активной
	glBindVertexArray(vertexArray);// делаем активным вершинный массив
	glDrawArrays(GL_LINE_STRIP, 0, 3);// отрисовка одного треугольника
	glBindVertexArray(0);*/

	while (!glfwWindowShouldClose(window)) { // пока окно window не должно закрыться
		glClearColor(0.2f, 0.3f, 0.3f, 1.0f);// назначаем цвет заливки
		glClear(GL_COLOR_BUFFER_BIT);        // очищаем буфер заданным цветом

		/*//ОТРИСОВКА ПРИМИТИВОВ
		glUseProgram(shaderProgram);// шейдерную программу shaderProgram делаем активной
		glBindVertexArray(vertexArray);// делаем активным вершинный массив
		glLineWidth(6); // устанавливаем толщину линии - 6
		glDrawArrays(GL_LINE_STRIP, 0, 3);// отрисовка одного треугольника
		glBindVertexArray(0);*/

		//ОТРИСОВКА СЦЕНЫ
		glUseProgram(shaderProgram);// шейдерную программу shaderProgram делаем активной
		glm::mat4 proj; // матрица перехода в пространство отсечения
		switch (pType) {
		case Ortho: // прямоугольная проекция
			proj = glm::ortho(l, r, b, t, n, f);
			break;
		case Frustum: // перспективная проекция с Frustum
			proj = glm::frustum(l, r, b, t, n, f);
			break;
		case Perspective: // перспективная проекция с Perspective
			proj = glm::perspective(fovy_work, aspect_work, n, f);
			break;
		}

		glm::mat4 C = proj * T; // матрица перехода от мировых координат в пространство отсечения
		for (int k = 0; k < models.size(); k++) {// цикл по рисункам
			std::vector<path> figure = models[k].figure; // список ломаных очередного рисунка
			glm::mat4 TM = C * models[k].modelM;// матрица общего преобразования рисунка
			// пересылка пересылка матрицы TM в переменную clipView шейдерной программы
			glUniformMatrix4fv(clipViewLocation, 1, GL_FALSE, glm::value_ptr(TM));
			for (int i = 0; i < figure.size(); i++) {
				// пересылка цвета линии в переменную pathColor шейдерной программы
				glUniform3fv(pathColorLocation, 1, glm::value_ptr(figure[i].color));
				glBindVertexArray(figure[i].vertexArray);// делаем активным вершинный массив i-й ломаной
				glLineWidth(figure[i].thickness);// устанавливаем толщину линии
				glDrawArrays(GL_LINE_STRIP, 0, figure[i].vertices.size());// отрисовка ломаной
				glBindVertexArray(0);// отключаем вершинный массив
			}
		}
		glfwSwapBuffers(window);             // поменять местами буферы изображения
		glfwPollEvents();                    // проверить, произошли ли какие-то события
	}

	glfwTerminate(); // завершить работу GLFW
	return 0;
}