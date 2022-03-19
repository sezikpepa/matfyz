class Cache():
    def __init__(self):
        self.cache = {}

    def add(self, key, value):
        self.cache[key] = value

    def test_inside(self, key):
        try:
            to_return = self.cache[key]
            return to_return
        except:
            return None

def number_of_sowing(flowerbed_count, previous_flowerbed="mrkev"):
    if cache.test_inside(flowerbed_count):
        return cache.test_inside(flowerbed_count)
    if flowerbed_count == 0:
        return 1
    count = 0
    #zasetá mrkev
    try:
        number = number_of_sowing(flowerbed_count - 1, "mrkev")
        cache.add(flowerbed_count, number)
        count += number
    except:
        pass

    if previous_flowerbed != "petrzel":
        try:
            number = number_of_sowing(flowerbed_count - 1, "petrzel")
            #cache.add(flowerbed_count, number)
            count += number 
        except:
            pass

    return count

vstup = int(input())
cache = Cache()
print(number_of_sowing(vstup))