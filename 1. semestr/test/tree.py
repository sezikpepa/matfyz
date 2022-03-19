

import os

def tree(cislo_slozky, path):
    obj = os.scandir(path)
    obj = sorted(obj, key=lambda element: element.name)
    for entry in obj:
        if entry.is_file():
            i = 0
            while (i != cislo_slozky):
                print('\t', end = "")
                i += 1
            print(entry.name)
        if entry.is_dir():
            i = 0
            while (i != cislo_slozky):
                print('\t', end = "")
                i += 1
            print(entry.name, end = "")
            print("/")
            new_path = path + "/" + entry.name
            tree(cislo_slozky + 1, new_path)
            
    #obj.close()

def main():
    path = os.getcwd()
    tree(0, path)

if __name__ == '__main__':
    main()
