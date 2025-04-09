from AppOpener import open, close, give_appnames

def AssistandOpenOrClose(cmd:str):
    cmd = cmd.lower()
    if cmd.startswith("close") == True:
      app_name = cmd.replace("close ","")
      close(app_name, match_closest=True)
    elif cmd.startswith("open") == True:
      app_name = cmd.replace("open ","")
      open(app_name, match_closest=True)
    elif cmd == "list":
      open("ls", match_closest=True)