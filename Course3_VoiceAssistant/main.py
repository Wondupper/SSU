import stt
import DetApps
import filter
import random
import datetime
import tts
import configs.jokes as jokes
from num2t4ru import num2text


apps = list(DetApps.give_appnames(upper=False))

def va_respond(voice: str):
    cmd = filter.filter_cmd(voice,apps)
    if cmd != None:
        if cmd.startswith("help") ==True:
            text = "Я умею: ..."
            text += "открывать и закрывать приложения и выводить их список ..."
            text += "рассказывать анекдоты"
            tts.va_speak(text)
            pass
        elif cmd.startswith('time') ==True:
            now = datetime.datetime.now()
            text = "Сейчас " + num2text(now.hour) + " " + num2text(now.minute)
            tts.va_speak(text)
        elif cmd.startswith("joke") ==True:
            tts.va_speak(random.choice(jokes.VA_JOKES))
        elif cmd.startswith('open') == True or cmd.startswith('close') == True or cmd.startswith('list') == True:
            tts.va_speak("Манипулирую с приложением")
            DetApps.AssistandOpenOrClose(cmd)
        else:
            tts.va_speak("Команда не распознана")
    else:
        tts.va_speak("Команда не распознана")


tts.va_speak("Слушаю вашу команду повелитель")
stt.va_listen(va_respond)