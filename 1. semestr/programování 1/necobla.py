class prvek:
    def __init__(self,hod = 0,next=None):
        self.hod=hod
        self.next=next

class linkedList:
    def __init__(self):
        self.seznam=None
    def member(self,co):            
        pom = self.seznam
        while pom != None:
            if pom.hod == co:
                return pom
            pom = pom.next
        return None

    def insert(self, co):            
        if self.seznam == None:             
           self.seznam = prvek(co)
        else:
            pom = self.seznam
            if co >= pom.hod:                               
                self.seznam = prvek(co, self.seznam)
            else:
                while pom.next != None and pom.next.hod > co:
                    pom = pom.next
                pom.next = prvek(co, pom.next)

    
    def print(self, actual):
        if actual is not None:
            print(actual.hod)
            self.print(actual.next)

    def delete(self, co):
        deleted = 0
        pom = self.seznam
        if self.seznam == None:
            return
        if self.seznam.hod == co:
            self.seznam = self.seznam.next
        else:

            while pom.next.hod != co:
                pom = pom.next
                if pom.next == None:
                    return

            if pom.next.next == None:
                pom.next = None
            else:
                pom.next.hod = pom.next.next.hod
                pom.next = pom.next.next
        
            
            

    def setrid(self):
        pom = self.seznam
        setrideno = 0
        while setrideno == 0:
            
            if pom.hod < pom.next.hod:
                pom.hod, pom.next.hod = pom.next.hod, pom.hod
                pom = pom.next
            else:
                setrideno = 1
            
            if pom.next == None or pom.hod > pom.next.hod:
                pom = self.seznam

        


l=linkedList()
b = 0
 
l1 = [int(i) for i in input().split()]
a  = l1[0]
if a == 1 or a == 2: 
    b = l1[1]
while 0<a<6:

    a = l1[0]
    if a == 1 or a == 2:
        b = l1[1]


    if a == 1:
        l.insert(b)
        try:
            l1=[]
            l1 = [int(i) for i in input().split()]
        except:
            pass

    elif a == 2:
    
        l.delete(b)
        try:
            l1=[]
            l1 = [int(i) for i in input().split()]
        except:
            pass
    elif a == 4:
        
        try:
            l1=[]
            l1 = [int(i) for i in input().split()]
        except:
            pass
    elif a == 5:
    
        l.print(l.seznam)
        try:
            l1=[]
            l1 = [int(i) for i in input().split()]
        except:
            pass