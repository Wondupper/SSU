#pragma once
#include <fstream>
#include <sstream>
#include <vector>
#include "Matrix.h"
#include "Transform.h"

namespace KG
{

	using namespace System;
	using namespace System::ComponentModel;
	using namespace System::Collections;
	using namespace System::Windows::Forms;
	using namespace System::Data;
	using namespace System::Drawing;
	using namespace std;

	vec3 Vc; // ������ ������� �� �����������
	vec3 V; // ������ ������� �� ���������
	//vector<path> figure;
	mat4 T; // �������, � ������� ������������� ��� ��������������
	// ������������� - ��������� �������
	mat3 T0 = mat3(1.f);
	mat4 initT; // ������� ���������� ��������������
	vec3 Vc_work, V_work; // ������� ��������� ��������������


	/// <summary>
	/// ������ ��� MyForm
	/// </summary>
	public ref class MyForm : public System::Windows::Forms::Form
	{
	public:
		MyForm(void)
		{
			InitializeComponent();
			//
			//TODO: �������� ��� ������������
			//
		}

	protected:
		/// <summary>
		/// ���������� ��� ������������ �������.
		/// </summary>
		~MyForm()
		{
			if (components)
			{
				delete components;
			}
		}


	protected:

	protected:


	private:
		/// <summary>
		/// ������������ ���������� ������������.
		/// </summary>
		System::ComponentModel::Container^ components;

#pragma region Windows Form Designer generated code
		/// <summary>
		/// ��������� ����� ��� ��������� ������������ � �� ��������� 
		/// ���������� ����� ������ � ������� ��������� ����.
		/// </summary>
		void InitializeComponent(void)
		{
			this->SuspendLayout();
			// 
			// MyForm
			// 
			this->AllowDrop = true;
			this->AutoScaleDimensions = System::Drawing::SizeF(6, 13);
			this->AutoScaleMode = System::Windows::Forms::AutoScaleMode::Font;
			this->AutoScroll = true;
			this->ClientSize = System::Drawing::Size(284, 261);
			this->Cursor = System::Windows::Forms::Cursors::WaitCursor;
			this->DoubleBuffered = true;
			this->Enabled = false;
			this->KeyPreview = true;
			this->MinimumSize = System::Drawing::Size(155, 120);
			this->Name = L"MyForm";
			this->RightToLeftLayout = true;
			this->Text = L"MyForm";
			this->UseWaitCursor = true;
			this->Load += gcnew System::EventHandler(this, &MyForm::MyForm_Load);
			this->Paint += gcnew System::Windows::Forms::PaintEventHandler(this, &MyForm::MyForm_Paint);
			this->KeyDown += gcnew System::Windows::Forms::KeyEventHandler(this, &MyForm::MyForm_KeyDown);
			this->Resize += gcnew System::EventHandler(this, &MyForm::MyForm_Resize);
			this->ResumeLayout(false);

		}
#pragma endregion
	private: float left = 30, right = 100, top = 20, bottom = 50; // ���������� �� ������ ����
		   float minX = left, maxX; // �������� ��������� ��������� x
		   float minY = top, maxY; // �������� ��������� ��������� y
		   float Wcx = left, Wcy; // ���������� ������ ������� ���� ��������������
		   float Wx, Wy; // ������ � ������ ��������������
		   float Wx_work, Wy_work; // ������ � ������ ������� ������ ����� ����� �������
		   float Wx_part = 0.6, Wy_part = 0.6;// ���� Wx_work, Wy_work �� Wx, Wy 
		   float Wcx_work, Wcy_work; // ���������� ������ ������� ���� ������ �������� ��������������
		   float Wz_work;  // ���������� ������� ���������������
		   int numXsect = 5, numYsect = 5, numZsect = 5; // ���������� ������ ������������ ����� �� ����
	private: System::Void rectCalc() {
		maxX = ClientRectangle.Width - right; // �������� ��������� ��������� x
		maxY = ClientRectangle.Height - bottom; // �������� ��������� ��������� y
		Wcy = maxY; // ���������� ������ ������� ���� ��������������
		Wx = maxX - left; // ������ ��������������
		Wy = maxY - top; // ������ � ������ ��������������
		// ���������� ������ � ������ �������� ��������������
		Wx_work = Wx_part * Wx;
		Wy_work = Wy_part * Wy;
		// ���������� ��������� ������� ������ ���� ������ �������� �������� ��������������
		Wcx_work = maxX - Wx_work;
		Wcy_work = minY + Wy_work;
		// ���������� ������� ���������������
		Wz_work = Wcy - Wcy_work;
	}
	private: System::Void worldRectCalc() {
		Vc_work = normalize(T * vec4(Vc, 1.f));
		V_work = mat3(T) * V;
	}
	private: float f(float x, float z) {
		return x * sin(sqrt(x * x + z * z));
	}
	private: bool f_exists(float x, float z, float delta) {
		return true;
	}

	private: System::Void MyForm_Paint(System::Object^ sender, System::Windows::Forms::PaintEventArgs^ e) {
		Graphics^ g = e->Graphics;
		g->Clear(Color::Aquamarine);

		Pen^ rectPen = gcnew Pen(Color::Black, 2);
		g->DrawRectangle(rectPen, left, top, Wx, Wy);

		Pen^ gridPen = gcnew Pen(Color::Black, 1);
		SolidBrush^ drawBrush = gcnew SolidBrush(Color::Black);
		System::Drawing::Font^ drawFont = gcnew System::Drawing::Font("Arial", 8);

		//�� �
		float gridStep_x = Wx_work / numXsect; // ���������� ����� ������� ����� �� x
		float grid_dX = V_work.x / numXsect;  // ���������� ����� ������� ����� �� x � ������� �����������
		//���� � ������� ����������� �����
		float tick_x = Vc_work.x; // �������� ��������������� ������ ����� �����
		for (int i = 0; i <= numXsect; i++) {// ���� �� ���������� �����
			float tmpXCoord_d = Wcx + i * gridStep_x;// ������ ���������� x i-� ������������ �����
			float tmpXCoord_v = Wcx_work + i * gridStep_x;// ���������� x i-� ������������ �����
			// i-� ������������ �����
			g->DrawLine(gridPen, tmpXCoord_d, Wcy, tmpXCoord_v, Wcy_work);
			// i-� ������������ �����
			g->DrawLine(gridPen, tmpXCoord_v, Wcy_work, tmpXCoord_v, minY);
			if (i > 0 && i < numXsect) // ���� ����� �� �������
				// ������� ����� � ������ ����� ������������ �����
				g->DrawString(tick_x.ToString("F4"), drawFont, drawBrush, tmpXCoord_d, Wcy);
			tick_x += grid_dX;// ��������� ��������, ��������� �����
		}


		//�� z
		gridStep_x = (Wx - Wx_work) / numZsect;// ���������� ����� ������������� ������� ����� �� �����������
		float gridStep_y = Wz_work / numZsect;
		float grid_dZ = V_work.z / numZsect;

		float tick_z = Vc_work.z;
		for (int i = 0; i <= numZsect; i++) {
			float tmpXCoord_v = Wcx_work - i * gridStep_x;// ���������� x ������������ �����
			float tmpYCoord_g = Wcy_work + i * gridStep_y;// ���������� y �������������� �����
			float tmpXCoord_g = tmpXCoord_v + Wx_work;// ������ ���������� x �������������� �����
			// i-� ������������ �����
			g->DrawLine(gridPen, tmpXCoord_v, tmpYCoord_g, tmpXCoord_v, tmpYCoord_g - Wy_work);
			// i-� �������������� �����
			g->DrawLine(gridPen, tmpXCoord_v, tmpYCoord_g, tmpXCoord_g, tmpYCoord_g);
			if (i > 0 && i < numZsect)
				g->DrawString(tick_z.ToString("F4"), drawFont, drawBrush, tmpXCoord_g, tmpYCoord_g);
			tick_z += grid_dZ;
		}

		//�� y
		float gridStep_z = Wy_work / numYsect; // ���������� ����� ������� ����� �� z
		float grid_dY = V_work.y / numYsect; //��������� ����� ������� ����� �� y � ��

		float tick_y = Vc_work.y;
		for (int i = 0; i <= numYsect; i++) {
			float tmpXCoord_v = Wcx_work; //����� � �� ���������
			float tmpYCoord_v = Wcy_work - i * gridStep_z;//����� � �� ���������
			float tmpYCoord_g = tmpYCoord_v + Wz_work; //����� � �� �����������
			g->DrawLine(gridPen, tmpXCoord_v, tmpYCoord_v, minX, tmpYCoord_g);
			g->DrawLine(gridPen, tmpXCoord_v, tmpYCoord_v, maxX, tmpYCoord_v);
			if (i > 0 && i < numYsect)
				g->DrawString(tick_y.ToString("F4"), drawFont, drawBrush, maxX, tmpYCoord_v);
			tick_y += grid_dY;
		}

		Pen^ pen = gcnew Pen(Color::Blue, 1);
		float deltaX = V_work.x / Wx_work;
		float deltaZ = V_work.z / Wz_work;

		float deltaWcx = (Wcx_work - Wcx) / Wz_work;

		bool hasStart;

		// ���� �� ���������������
		float z = Vc_work.z; // ���������� z ��������������� �������� ��������������
		// ���������� ������ ������� ���� �������� ��������������
		float Wcx_w = Wcx_work, Wcy_w = Wcy_work;
		while (Wcy_w <= Wcy) {// ���� �� ��������� ��� ��������������
			vec2 start, end;  //��� ������ ������� � ����������� ������
			float x, y; //�����. ���. � ������� ��
			start.x = Wcx_w;// ��� ��������� ����� 1�� ������� ������������� ���������� x
			x = Vc_work.x; // ���������� x ��������� ����� 1�� ������� � ������� �����
			hasStart = f_exists(x, z, deltaX);
			if (hasStart) {
				y = f(x, z); //����� x ��� ��� � ������� ��
				start.y = Wcy_w - (y - Vc_work.y) / V_work.y * Wy_work;//�������� � ����� ������
			}
			float maxX = Wcx_w + Wx_work;
			//���� �� ��� ���, ���� ����� � �� ��������� ������� ���� ���� �� �����
			while (start.x < maxX) {
				vec2 end; // ����� ����� ������� � ����������� ������
				bool hasEnd;
				float deltaY; // ������ ����� � �������������� (���� ����� ������)
				float red, green, blue; // ���������� ����� �������
				end.x = start.x + 1.f;
				x += deltaX;
				hasEnd = f_exists(x, z, deltaX);
				if (hasEnd) {
					y = f(x, z);
					deltaY = (y - Vc_work.y) / V_work.y;
					end.y = Wcy_w - deltaY * Wy_work;
				}
				//�������� ��������� ������� ������������ ������� ��������� �� �����
				vec2 tmpEnd = end;
				bool visible = hasStart && hasEnd && clip(start, end, Wcx_w, Wcy_w-Wy_work, Wcx_w + Wx_work, Wcy_w);
				if (visible) {// ���� ������� �����
					// ����� ���������, start � end - ����� ������� ����� �������
					if (deltaY > 1.f) deltaY = 1.f; // ����������� �������� ������ �����
					if (deltaY < 0.f) deltaY = 0.f; // �� ������, ���� ������� ���������
					green = 510.f * deltaY; ; // ��������������� ���������� ������������
					if (deltaY < 0.5) { // ���� ����� ���� �������� ������� ���������
						blue = 255.f - green; // ����� ��������� �������
						red = 0.f; // ������� ����� ����
					}
					else {// ���� ����� �� ���� ��������
						blue = 0.f; // ����� ����� ����
						red = green - 255.f; // ��������� ������� � �������
						green = 510.f - green; // � �������������� ������������ ������������
					}
					pen->Color = Color::FromArgb(red, green, blue); // ������ ���� ����
					g->DrawLine(pen, start.x, start.y, end.x, end.y);// ��������� ������� ������
				}
				// �������� ����� ������������� ������� ���������� ��������� ������ ����������
				start = tmpEnd;
				hasStart = hasEnd;
			}
			Wcy_w += 1.f; // ��������� � ���������� ��������������, �� ����� ���� �� ���� ������
			Wcx_w -= deltaWcx; // � ����� �� ��������� ��������
			z += deltaZ;//��������� z ��� ��������� ��������������
		}
	}
	private: System::Void MyForm_KeyDown(System::Object^ sender, System::Windows::Forms::KeyEventArgs^ e) {
		float centerX = Vc_work.x + V_work.x / 2;//����� ������
		float centerY = Vc_work.y + V_work.y / 2;//� ������� ��
		float centerZ = Vc_work.z + V_work.z / 2;
		switch (e->KeyCode) {
		case Keys::Escape:
			T = initT; // ��������� T ��������� �������
			Wx_part = 0.6;
			Wy_part = 0.6;
			rectCalc();
			numXsect = 5, numYsect = 5, numZsect = 5;
			break;
		case Keys::X:
			T = translate(-centerX, -centerY, -centerZ) * T;
			T = scale(1.f / 1.1f, 1.f / 1.1f, 1.f / 1.1f) * T;
			T = translate(centerX, centerY, centerZ) * T;
			break;

		case Keys::A:
			T = translate(-V_work.x / Wx, 0.f, 0.f) * T; 
			break;

		case Keys::Z:
			T = translate(-centerX, -centerY, -centerZ) * T; 
			T = scale(1.1, 1.1, 1.1) * T; 
			T = translate(centerX, centerY, centerZ) * T; 
			break;

		case Keys::D:
			T = translate(V_work.x / Wx, 0.f, 0.f) * T;
			break;
		case Keys::W:
			T = translate(0.f, 0.f, V_work.x / Wx) * T;
			break;
		case Keys::S:
			T = translate(0.f, 0.f, -V_work.x / Wx) * T;
			break;

		case Keys::R:
			T = translate(0.f, -V_work.x / Wx, 0.f) * T;
			break;
		case Keys::F:
			T = translate(0.f, V_work.x / Wx, 0.f) * T;
			break;

		case Keys::Q:
			if (Wx_part <= 0.9 ) Wx_part *= 1.1;
			rectCalc();
			break;
		case Keys::E:
			if (Wx_part >= 0.2) Wx_part /= 1.1;
			rectCalc();
			break;

		case Keys::C:
			if (Wy_part <= 0.9 ) Wy_part *= 1.1;
			rectCalc();
			break;
		case Keys::V:
			if ( Wy_part >= 0.2) Wy_part /= 1.1;
			rectCalc();
			break;

		case Keys::T:
			T = translate(-centerX, -centerY, -centerZ) * T;
			T = scale(1.1, 1., 1.) * T;
			T = translate(centerX, centerY, centerZ) * T;
			break;
		case Keys::G:
			T = translate(-centerX, -centerY, -centerZ) * T;
			T = scale(1. / 1.1, 1., 1.) * T;
			T = translate(centerX, centerY, centerZ) * T;
			break;

		case Keys::Y:
			T = translate(-centerX, -centerY, -centerZ) * T;
			T = scale(1., 1.1, 1.) * T;
			T = translate(centerX, centerY, centerZ) * T;
			break;
		case Keys::H:
			T = translate(-centerX, -centerY, -centerZ) * T;
			T = scale(1., 1. / 1.1, 1.) * T;
			T = translate(centerX, centerY, centerZ) * T;
			break;

		case Keys::U:
			T = translate(-centerX, -centerY, -centerZ) * T;
			T = scale(1., 1., 1.1) * T;
			T = translate(centerX, centerY, centerZ) * T;
			break;
		case Keys::J:
			T = translate(-centerX, -centerY, -centerZ) * T;
			T = scale(1., 1., 1. / 1.1) * T;
			T = translate(centerX, centerY, centerZ) * T;
			break;

		case Keys::D1:
			numXsect++;
			break;
		case Keys::D2:
			if (numXsect > 2)numXsect--;
			break;
		case Keys::D3:
			numYsect++;
			break;
		case Keys::D4:
			if (numYsect > 2)numYsect--;
			break;
		case Keys::D5:
			numZsect++;
			break;
		case Keys::D6:
			if (numZsect > 2)numZsect--;
			break;
		}
		worldRectCalc();
		Refresh();
	}
	private: System::Void MyForm_Resize(System::Object^ sender, System::EventArgs^ e) {
		rectCalc();
		Refresh();
	}

	private: System::Void MyForm_Load(System::Object^ sender, System::EventArgs^ e) {
		Vc = vec3(-2.f, -2.f, -2.f);
		V = vec3(4.f, 4.f, 4.f);
		initT = mat4(1.f);
		T = initT;
		rectCalc();
		worldRectCalc();
	}
	};
}
