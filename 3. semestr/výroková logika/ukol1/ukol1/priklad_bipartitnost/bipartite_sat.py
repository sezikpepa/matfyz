import sys
import itertools
import math

vertices = []
edges = []

def read_graph(filename):
	with open(filename) as f:
		vert_num = -1
		edg_num = -1
		for line in f.readlines():
			if line.startswith('c '): # ignore comments
				continue
			if line.startswith('e '): # add an edge, check vertex number are consistent
				parts = line.split(' ')
				u, v = int(parts[1]), int(parts[2])
				if u > vert_num or v > vert_num:
					print('Warning: invalid vertex number found in edge:', line)
				edges.append((u, v))
				
			if line.startswith('p edge'): # parse problem specification
				parts = line.split(' ')
				vert_num = int(parts[2])
				edg_num = int(parts[3])
				vertices = list(range(1, vert_num + 1))

		if edg_num != len(edges):
			print('Warning: number of edges does not match file header: %d != %d' % (len(edges), edg_num))

	return vertices, edges

def write_cnf(cnf, filename):

	variables =  max(map(abs, itertools.chain(*cnf))) # find the maximum number of a variable used
	cnf_str = '\n'.join(map(lambda c: ' '.join(map(str, c)) + ' 0', cnf)) # concatenate clauses into a string

	print('CNF created, it has %d variables and %d clauses' % (variables, len(cnf)))

	with open(filename, 'w') as f:
		f.write('p cnf %d %d\n' % (variables, len(cnf))) # write basic CNF information
		f.write(cnf_str)

def get_literal(vertex, edge):
	return int(f"{vertex}0{edge}")
	

def generate_cnf(vertices, edges, k):
	clauses = []

	#every vertex has at least one color
	for vertex in vertices:
		clause = []
		for i in range(k):
			clause.append(get_literal(vertex, i + 1))
		clauses.append(clause)
	#--------------------------------------------------------------------------------

	#every vertex has maximum one color
	for vertex in vertices:		
		for i in range(math.ceil(k / 2)):
			clause = []
			clause.append(-get_literal(vertex, i + 1))
			for j in range(k):		
				if(i != j):		
					clause.append(-get_literal(vertex, j + 1))
					clauses.append(clause)


				clause = []
				clause.append(-get_literal(vertex, i + 1))
			

	#connected verticies do not have the same color
	for color in range(k):		
		for i,j in edges:
			clause = []
			clause.append(-get_literal(i, color + 1))
			clause.append(-get_literal(j, color + 1))

			clauses.append(clause)

	#for i,j  in edges:  # for each edge, both vertices need to have a different color
		#clauses.append([-i, -j])
		#clauses.append([i, j])

	return list(set(clauses))

if __name__ == '__main__':
	vertices, edges = read_graph(sys.argv[1])

	print('Number of vertices:', len(vertices))
	print('Number of edges:', len(edges))

	cnf = generate_cnf(vertices, edges, int(sys.argv[2]))

	write_cnf(cnf, sys.argv[1] + '.cnf')