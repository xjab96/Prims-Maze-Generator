# Prims Maze Generator
 Uses Prims alogorithm to generate a random maze.
# Explanation
 This article explains it extremely well:
 https://en.wikipedia.org/wiki/Prim%27s_algorithm
 </break>I saw the algorithm one day and felt like coding it.
  
# Current implementation
  The current implementation isn't ideal for this type of algorithm. This is because short paths are fairly common and can end up fairly obvious see below. To reduce this obviousness it would be better to add space between the walls to some extent.
  ![alt text](https://i.imgur.com/JaMvlSQ.png "Maze")
  
# Reducing random
 The maze is inherently random due to the nature of the algorithm. Randomness if not done well can go infinitely. To reduce random to a finite number I have used open and closed lists where possible removing from the open list when a piece is completed. Thus removing the chance for the piece to be picked again infinitely.
 
# Plans for the future
 I wish to make this work for node maps as well as work on a better, standard maze where the short paths happen less and are less obvious.
 
