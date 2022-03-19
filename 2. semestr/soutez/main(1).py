
class Contributor:
    def __init__(self, name, skills):
        self.name = name
        self.skills = skills

class Skill:
    def __init__(self, name, level):
        self.name = name
        self.level = level

class Project:
    def __init__(self, name, n_of_days, score, best_before, roles):
        self.name = name
        self.n_of_days = n_of_days
        self.score = score
        self.best_before = best_before
        self.roles = roles

def get_contributors(lines, n_of_contributors):
    contributors = []

    index = 1
    while len(contributors) < n_of_contributors:
        skills = []

        name = lines[index].strip().split(" ")[0]
        n_of_skills = int(lines[index].strip().split(" ")[1])

        index += 1

        for i in range(n_of_skills):
            skill_name = lines[index + i].strip().split(" ")[0]
            skill_level = int(lines[index + i].strip().split(" ")[1])

            skills.append(Skill(skill_name, skill_level))

        index += n_of_skills

        contributors.append(Contributor(name, skills))

    return [ contributors, index ]

def get_projects(lines, n_of_projects, index):
    projects = []

    while len(projects) < n_of_projects:
        name = lines[index].strip().split(" ")[0]
        n_of_days = int(lines[index].strip().split(" ")[1])
        score = int(lines[index].strip().split(" ")[2])
        best_before = int(lines[index].strip().split(" ")[3])
        n_of_roles = int(lines[index].strip().split(" ")[4])

        roles = []

        index += 1

        for i in range(n_of_roles):
            skill_name = lines[index + i].strip().split(" ")[0]
            skill_level = int(lines[index + i].strip().split(" ")[1])

            roles.append(Skill(skill_name, skill_level))

        index += n_of_roles

        projects.append(Project(name, n_of_days, score, best_before, roles))

    return projects

def get_input():
    file = open("input.txt", "r")
    lines = file.readlines()

    n_of_projects = int(lines[0].strip().split(" ")[0])
    n_of_contributors = int(lines[0].strip().split(" ")[1])

    gc = get_contributors(lines, n_of_contributors)
    contributors = gc[0]
    index = gc[1]
    projects = get_projects(lines, n_of_projects, index)

    return [ contributors, projects ]



data = get_input()
contributors = data[0]
projects = data[1]

for contributor in contributors:
    print("Contributor name:", contributor.name)

    for skill in contributor.skills:
        print("Skill name:", skill.name)
        print("Skill level:", skill.level)

print("--------------------")

for project in projects:
    print("Project name:", project.name)
    print("Project n-of-days:", project.n_of_days)
    print("Project score:", project.score)
    print("Project best-before:", project.best_before)

    for skill in project.roles:
        print("Skill name:", skill.name)
        print("Skill level:", skill.level)