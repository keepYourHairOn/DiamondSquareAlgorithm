# DiamondSquareAlgorithm
The project created for the course Procedural Content Generation (PCG).
Diamond-square is a random midpoint displacement fractal algorithm, used for height map generation. 

The algorithm work by division of the terrain on particles until the size of particle comes to negligible size, in the case 1 pixel. The algorithm works in such a manner:
1.	Start with the square of the initial terrain size. Assign each corner of the grid with random values (heights).
2.	The Square step. Find the midpoint of the square as average of corners with random diaplacement Fig. 1 (Square). Make sure that midpoint is not out of range, otherwise recalculate it.
3.	The Diamond step. Find the midpoint of the diamond to be the average of four edges with random diaplacement Fig. 1 (Diamond).
4.	Repeat step 2 for each square and step 3 for each diamond until square size will be equale to 1 pixel.

