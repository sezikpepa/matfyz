class VrcholBinStromu:
    """třída pro reprezentaci vrcholu binárního stromu""" 
    def __init__(self, info = None, levy = None, pravy = None)
        self.info = info      # data
        self.levy = levy      # levé dítě 
        self.pravy = pravy    # pravé dítě

def cesta(koren : VrcholBinStromu, x : int, y : int) -> List:
    pass
    """
    koren : kořen zadaného binárního stromu
    x,y   : hodnoty koncových vrcholů hledané cesty
    vrátí : seznam čísel vrcholů na cestě z x do y
    """

def najdi_vrchol(koren: VrcholBinStromu, x: int, cesta_do_vrcholu: list): #prohledáním do hloubky naleznu cestu do vrcholů
    if koren.info == x:
        return cesta_do_vrcholu

    else:
        najdi_vrchol(koren.levy, cesta_do_vrcholu + koren.levy.info})
        najdi_vrchol(koren.pravy, cesta_do_vrcholu + koren.pravy.info})

def najdi_cestu(x, y):
    cesta_do_x = najdi_vrchol(koren, x, "")
    cesta_do_y = najdi_vrchol(koren, y, "")

    if not (cesta_do_x or cesta_do_y): #pokud neexistuje cesta do jednoho nebo obou vrcholů, vrátí se prázdný seznam
        return []

    cesta_mezi_vrcholy = []
    start_index = 0
    if not (len(cesta_do_y) > len(cesta_do_x)):  #hledání, zda se začátek cesty neshoduje, tím pádem ho nemusím již prohledávat
        for i in range(len(cesta_do_x)):
            if cesta_do_y[i] == cesta_do_x[i]:
                start_index = i
            else:
                break
        cesta_do_y, cesta_do_x = cesta_do_x, cesta_do_y #chci aby v proměnné cesta_do_y byla delší cesta, mohu to udělat, protože u cestty nezáleží na směru, pokud by záleželo, tak stačí výsledek vypsat v opačnéhom pořadí

    for i in range(start_index, len(cesta_do_y)): #procházím od vrcholu, který je v nižší hladině výš, dokud nenaleznu x, nebo začátek cesty do x
        char = cesta_do_y[-(i + 1)]

        if cesta_do_x[0] == char:
            for i in range(len(cesta_do_x)): #doplnění cesty od cestu do x
                if cesta_do_x[i] != x:
                    cesta_mezi_vrcholy.append(cesta_do_x[i]) 
                else:
                    break

            return cesta_mezi_vrcholy

        elif cesta_do_x[0] == x:
            return cesta_mezi_vrcholy
        else:
            cesta_mezi_vrcholy.append(char)



