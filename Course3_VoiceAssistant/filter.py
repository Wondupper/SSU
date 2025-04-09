from strsimpy.normalized_levenshtein import NormalizedLevenshtein
import configs.prepositions as prepositions

def filter_cmd(cmd: str,apps):
    cmd = cmd.strip()
    words = delete_preposotions(cmd.split())
    if len(words)>=2:
        firstPart = filter_firstPart1(words[0])
        secondPart = filter_secondPart(' '.join(words[1:]),apps)
        return ' '.join([firstPart,secondPart])
    elif len(words)==1:
        firstPart = filter_firstPart2(words[0])
        return firstPart
    else:
        return ""

def filter_firstPart1(firstPart:str):
    comands = ["open","close"]
    metrix= NormalizedLevenshtein()
    setter = dict()
    for el in comands:
        setter[el] = metrix.distance(el,firstPart)
    setter2 = dict(sorted(setter.items(), key=lambda item: item[1]))
    result = list(setter2.keys())[0]
    return result

def filter_firstPart2(firstPart:str):
    comands = ["list","help","joke","time"]
    metrix= NormalizedLevenshtein()
    setter = dict()
    for el in comands:
        setter[el] = metrix.distance(el,firstPart)
    setter2 = dict(sorted(setter.items(), key=lambda item: item[1]))
    result = list(setter2.keys())[0]
    return result

    
def filter_secondPart(secondPart:str,apps:list):
    metrix = NormalizedLevenshtein()
    setter = dict()
    for el in apps:
        setter[el] = metrix.distance(el,secondPart)
    setter2 = dict(sorted(setter.items(), key=lambda item: item[1]))
    return list(setter2.keys())[0]

def delete_preposotions(words:list):
    for el in words:
        if el in prepositions.VA_PREPOSITIONS:
            words.remove(el)
    return words

