# Snax_TestAssig
Test Assignment for apllying at Unity Developer position
Recommended setings:
  - Grid size: adjust both in PuzzleController.cs and GridMeshGenerator.cs inspectors (default 10)
  - Zones radiuses: adjust in ZoneStreamer.cs inspector, set enter radius lower than exit, exit at least 1 unit greater than half of the distance between zones centers (to have overlap area where both zones may be loaded simultoniously) (default 6/8) 
  - Zones centering: set point roughly equal to center of mass of objects prezent in subscene (default beyond farthermost corners of tile grid)
  - Adressables groups settings:
![screenshot](https://github.com/xflomasterx/Snax_TestAssig/blob/main/Adressables.png)
