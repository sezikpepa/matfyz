class Node:
    def __init__(self, value, next=None):
        self.value: float = value
        self.next: Node = next


class LinkedList:
    def __init__(self):
        self.head: Node = None

    def insert(self, element: float):
        if not self.head:
            node = Node(element, self.head)
            self.head = node
        elif element < self.head.value:
            node = Node(element, self.head)
            self.head = node

        else:
            current_node = self.head
            while True:
                if current_node.next is None:
                    current_node.next = Node(element)
                    break
                elif current_node.value <= element <= current_node.next.value:
                    node = Node(element, current_node.next)
                    current_node.next = node
                    break
                current_node = current_node.next

    def __str__(self):
        to_print = ""
        current_position = self.head
        while current_position is not None:
            to_print += str(current_position.value) + " "
            current_position = current_position.next

        return to_print
       
linked_list = LinkedList()   

line = input()
while line != "":
    for item in line.split(" "):
        if item != "":
            linked_list.insert(int(item))

    line = input()

print(linked_list)

