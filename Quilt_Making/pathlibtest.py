import pathlib
import numpy as np
import os
import urllib
from matplotlib import pyplot as plt

#GLOBAL
Picture_number_in_a_set = 0
Quilt_Height = 4096
Quilt_Width = 4096
tile_Height = 4096 // 5
tile_Width = 4096 // 5
MAX_Parallax = 45

Parallax_List = [45, 23, 15, 12, 9]
JPG_Quality_List = [90, 60, 30]

blank_image = np.zeros((Quilt_Height, Quilt_Width, 3), np.uint8)
Folder_List = ["elephant", "tree"]
currentDirectory = pathlib.Path("./elephant")
pattern = "*.png"

def main():
    Folder_Index = 0
    
    for Folder_Index in range(len(Folder_List)):
        cur_jpg_quality = JPG_Quality_List[0]
        cur_parallax_index = 0
        parallax_per_pic = 1
        RawImgPath = Folder_List[Folder_Index]
        
    #print(files)
    file_number = 0
    for files in currentDirectory.glob(pattern):
        file_number = file_number + 1
    print(file_number)


if __name__ == '__main__':
    main()



