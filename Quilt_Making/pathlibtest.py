import pathlib
import numpy as np
import cv2 as cv
import os
import urllib
from matplotlib import pyplot as plt

#GLOBAL
Picture_number_in_a_set = 0
Quilt_Height = 4096
Quilt_Width = 4096
tile_Height = 4096 // 9
tile_Width = 4096 // 5

Parallax_List = [45, 23, 15, 12, 9] # MAX_Parallax = 45
JPG_Quality_List = [90, 60, 30]

blank_image = np.zeros((Quilt_Height, Quilt_Width, 3), np.uint8)
Raw_Png_Directory = pathlib.Path("./UCSD/")
Folder_List = [x for x in Raw_Png_Directory.iterdir() if x.is_dir()] # read sub_folder in RawPng folder
pattern = "*.png"

def main():
    
    for Folder_Index in range(len(Folder_List)): # iteration through raw png folder, quality, and parallax
        png_input_list =  [png_raw for png_raw in Folder_List[Folder_Index].glob(pattern)]
        #current_Raw_Png_Directory = str(Raw_Png_Directory) + str(Folder_List[Folder_Index]) + "/"
        for JPG_Quality_Index in range(len(JPG_Quality_List)): # create 3 * 5 sets of stimuli
            for Parallax_List_Index in range(len(Parallax_List)): 
                parallax_per_pic = Parallax_List_Index + 1 # 1 ~ 5
                for grid_index in range(1, Parallax_List[0] + 1): # 1 ~ 45
                    feed_image_str = png_input_list[UCSD_DataSet_str_processing(len(png_input_list) // 2, parallax_per_pic, grid_index)]
                    img = cv.imread(str(feed_image_str))
                    print(str(feed_image_str))
                    canvas_paint(img, grid_index)
                img = jpg_codec(blank_image, JPG_Quality_List[JPG_Quality_Index])
                cv.imwrite("Quilt/" + str(Folder_List[Folder_Index]) + "/Tile_generate__" + str(JPG_Quality_List[JPG_Quality_Index]) + "__" + str(Parallax_List[Parallax_List_Index]) + ".jpg", img)

def UCSD_DataSet_str_processing(input_img_num, parallax_per_pic, grid_index):
    index_for_feed_image = (grid_index + parallax_per_pic - 1) // parallax_per_pic
    sampling_interval = input_img_num // Parallax_List[0]
    feed_image = 1 + (index_for_feed_image - 1) * sampling_interval * parallax_per_pic
    return feed_image

def canvas_paint(img, i):
    try:
        img = cv.resize(img ,(tile_Width, tile_Height))
    except Exception as e:
        print(str(e))
    i = 46 - i # The UCSD DataSet sweep scene from right to left
    j = 4 - (i - 1) % 5 if (i % 5 != 0) else 0 # Width Index
    k = i // 5 if (i % 5 != 0) else i // 5 -1 # Height Index
    blank_image[k * tile_Height : (k + 1) * tile_Height, j * tile_Width : (j + 1) * tile_Width] = img

def jpg_codec(img, jpg_quality):
    ret, buf = cv.imencode("Quilt/" + str(jpg_quality) + ".jpg", img, [int(cv.IMWRITE_JPEG_QUALITY), jpg_quality])
    img = cv.imdecode(buf, cv.IMREAD_COLOR)
    return img

if __name__ == '__main__':
    main()



