# Prims Maze Generator 

   ![alt text](https://i.imgur.com/XDfMsTe.png "Maze")
   
# Explanation
 This article explains it extremely well:
 https://en.wikipedia.org/wiki/Prim%27s_algorithm
 
 I saw the algorithm one day and felt like coding it. This by far isnt the best implementation. I shall continue to work on this and improve it however.
  
# Current implementation
  The current implementation isn't ideal for this type of algorithm. This is because short paths are fairly common and can end up fairly obvious see below. To reduce this obviousness it would be better to add space between the walls to some extent.
  
  ![alt text](https://i.imgur.com/JaMvlSQ.png "Maze short tunnel problem")
  
# Reducing random
 The maze is inherently random due to the nature of the algorithm. Randomness if not done well can go infinitely. To reduce random to a finite number I have used open and closed lists where possible removing from the open list when a piece is completed. Thus removing the chance for the piece to be picked again infinitely.
 
# Plans for the future
 I wish to make this work for node maps as well as work on a better, standard maze where the short paths happen less and are less obvious.
 
