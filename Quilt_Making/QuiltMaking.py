import numpy as np
import cv2 as cv
import urllib
import os
from matplotlib import pyplot as plt

# GLOBAL
height = 4096
width = 4096
tile_width = width // 5
tile_height = height // 5
MAX_PARALLAX = 45
# input_image_num = 193

# StepSize = 30
Parallax_matrix = [45, 23, 15, 12, 9]
JPG_quality_matrix = [90, 60, 30]

# assign a blank canvas for quilt
blank_image = np.zeros((height, width, 3), np.uint8)

Folder = ["Toys","Castle", "Dragon", "Flowers", "Holiday", "Seal and Balls"]

def main():
    folder_index = 0
    last_parallax_downgrade = 95
    
    for f in range(6):
        index = 0
        cur_jpg_quality = 95
        cur_parallax_index = 0
        parallax_per_pic = 1
        InputFolder = Folder[f]
        RawImgPath = "RawPng/DSLF - " + InputFolder + "/"
        img_for_size = cv.imread(RawImgPath + "0001.png")
        img_for_size = cv.resize(img_for_size ,(tile_width, tile_height))
        last_parallax_downgrade = cur_jpg_quality
        while (not (cur_parallax_index == 4 and cur_jpg_quality < 10)):
            # os.path.getsize('C:\\Python27\\Lib\\genericpath.py')
            jpg_save("temp/last.jpg", img_for_size, jpg_quality = last_parallax_downgrade)
            jpg_save("temp/cur.jpg", img_for_size, jpg_quality = cur_jpg_quality)
            size_cur = os.path.getsize("temp/cur.jpg")
            size_last = os.path.getsize("temp/last.jpg")
            print(size_cur)
            print(size_last)

            if (cur_parallax_index != 4):        
                if (size_cur * Parallax_matrix[cur_parallax_index] < size_last * Parallax_matrix[cur_parallax_index + 1]):
                    # cur_jpg_quality = 95
                    cur_parallax_index = cur_parallax_index + 1
                    PARALLAX = Parallax_matrix[cur_parallax_index]
                    parallax_per_pic = cur_parallax_index + 1
                    print(parallax_per_pic)
                    print(PARALLAX)
                    
                    #jpg quality of last parallax downgrade
                    last_parallax_downgrade = cur_jpg_quality
                else:
                    cur_jpg_quality = cur_jpg_quality - StepSize
            else:
                cur_jpg_quality = cur_jpg_quality - StepSize
            
            for i in range(1, Parallax_matrix[0] + 1):
                
                feed_image_str = DSLF_raw_imread(i, parallax_per_pic)

                img = cv.imread(RawImgPath + feed_image_str)
                print("*****" +Folder[f]+ feed_image_str)
                
                canvas_paint(img, i)

            img = jpg_codec(blank_image, cur_jpg_quality)
            cv.imwrite("Quilt/" + InputFolder + "/Tile_generate__" + str(index) + "__" + str(cur_jpg_quality) + "__" + str(Parallax_matrix[cur_parallax_index]) + ".jpg", img)
            index = index + 1

def canvas_paint(img, i):
    img = cv.resize(img ,(tile_width, tile_height))
    #j = i % 5 if (i % 5 != 0) else 5
    j = 4 - (i - 1) % 5 if (i % 5 != 0) else 0
    k = i // 5 if (i % 5 != 0) else i // 5 -1
    blank_image[k * tile_height : (k + 1) * tile_height, j * tile_width : (j + 1) * tile_width] = img
    #blank_image[k * tile_height : (k + 1) * tile_height, ((i - 1) % 5) * tile_width : j * tile_width] = img

def jpg_codec(img, jpg_quality):
    ret, buf = cv.imencode("Quilt/" + str(jpg_quality) + ".jpg", img, [int(cv.IMWRITE_JPEG_QUALITY), jpg_quality])
    img = cv.imdecode(buf, cv.IMREAD_COLOR)
    return img

def DSLF_raw_imread(i, j):
    sampling_interval = input_image_num // PARALLAX
    index_for_feed_image = (i + j - 1) // j
    feed_image = 1 + (index_for_feed_image - 1) * sampling_interval * j
    
    if (feed_image >= 100):
        feed_image_str = "0" + str(feed_image)
    elif (100 > feed_image and feed_image >= 10):
        feed_image_str = "00" + str(feed_image)
    else:
        feed_image_str = "000" + str(feed_image)

    return (feed_image_str + ".png")

def jpg_save(path, image, jpg_quality = None, png_compression = None):
    if (jpg_quality):
        cv.imwrite(path, image, [int(cv.IMWRITE_JPEG_QUALITY), jpg_quality])
    elif png_compression:
        cv.imwrite(path, image, [int(cv.IMWRITE_PNG_COMPRESSION), png_compression])
    else:
        cv.imwrite(path, image)

if __name__ == '__main__':
    main()
