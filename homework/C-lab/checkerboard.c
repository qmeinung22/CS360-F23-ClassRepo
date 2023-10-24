#include <stdio.h>
#include <stdlib.h>
#include <png.h>

// Function prototype
void set_image_data(png_bytep * data, int w, int h);

int main(int argc, char ** argv)
{
    //Make sure it's the correct number of arguments
    if (argc != 4)
    {
        printf("Usage: %s <output_file.png> <width> <height>\n", argv[0]);
        exit(EXIT_FAILURE);
    }

    char *output_file = argv[1];
    // Dimensions of image to create
    png_uint_32 width = atoi(argv[2]);  //Switched to png_uint_32
    png_uint_32 height = atoi(argv[3]); //Switched to png_uint_32

    // Open the file to write to.  If it exists it will be overwritten.
    FILE * fp = fopen(argv[1], "wb");
	if(!fp)
	{
		fprintf( stderr, "Can't open file %s", argv[1] );
		exit(EXIT_FAILURE);
	}

    // Initialize libPNG, obtaining pointers to a write struct, and an info struct that we will use later
    // These structs are allocated on the heap by libPNG
    png_structp png_ptr = png_create_write_struct(PNG_LIBPNG_VER_STRING, NULL, NULL, NULL);
    if(!png_ptr)
    {
        fprintf(stderr, "Could not create libpng write structure");
        exit(EXIT_FAILURE);
    }

    png_infop info_ptr = png_create_info_struct(png_ptr);
    if(!info_ptr)
    {
       png_destroy_write_struct(&png_ptr,(png_infopp)NULL);
       fprintf(stderr, "Could not create libpng info structure");
       exit(EXIT_FAILURE);
    }

    // Set up the error handling that libPNG expects
    if(setjmp(png_jmpbuf(png_ptr)))
    {
       png_destroy_write_struct(&png_ptr, &info_ptr);
       fclose(fp);
       fprintf(stderr, "An error occurred while libPNG was reading or writing data\n");
       exit(EXIT_FAILURE);
    }

    // Attach our file pointer so it knows where to write data to
    png_init_io(png_ptr, fp);

    // Specify dimensions, 8 bits per color, RGB, no interlacing, 
    png_set_IHDR(png_ptr, info_ptr, width, height, 8, PNG_COLOR_TYPE_RGB, PNG_INTERLACE_NONE, PNG_COMPRESSION_TYPE_DEFAULT, PNG_FILTER_TYPE_DEFAULT);

    // Add title and description
    //Code to generate title that reflects correct "Pwidthxheight format"
    int text_size = snprintf(NULL, 0, "P%dx%d", width, height);
    char *title_text = (char *)malloc(text_size + 1);
    sprintf(title_text, "P%dx%d", width, height);

    png_text text[2];
    text[0].compression = PNG_TEXT_COMPRESSION_NONE;
    text[0].key = "Title";
    text[0].text = title_text;
    text[1].compression = PNG_TEXT_COMPRESSION_NONE;
    text[1].key = "Author";
	text[1].text = "Quintan Meinung";
	png_set_text(png_ptr, info_ptr, text, 2);

    free(title_text);

    // Actually write the header info
    png_write_info(png_ptr, info_ptr);

    // Now we're ready to write the actual image data
    //   allocate an array, i.e. a column, of pointers
    png_bytep * row_ptrs = (png_bytep *)malloc(sizeof(png_bytep) * height);

    //Code to check if malloc fails to allocate memory
    if (row_ptrs == NULL)
    {
        fprintf(stderr, "Memory allocation failed for row_ptrs\n");
        exit(EXIT_FAILURE);
    }

    //   then have each of those point to an array for each row
    for(int j = 0; j < height; j++)
    {
        row_ptrs[j] = (png_bytep)malloc(sizeof(png_byte) * 3 * width);
        if (row_ptrs[j] == NULL)
        {
            fprintf(stderr, "Memory allocation failed for row_ptrs[%d]\n", j);
            exit(EXIT_FAILURE);
        }
    }
    
    // Now we're ready to create the image.  Write the data in row_ptrs in RGB format.
    set_image_data(row_ptrs, width, height);

    // Now write out this data to the file
    png_write_image(png_ptr, row_ptrs);

    // Finish writing (no more metadata or anything)
    png_write_end(png_ptr, NULL);

    // Clean up before exiting
    //"File deletion implemented by me<----------------"
    fclose(fp);

    // free each row
    for(int j = 0; j < height; j++)
    {
        free(row_ptrs[j]);
    }
    // then the array of row pointers
    //"This was implemented by me<-------------------"
    free(row_ptrs);

    // free all the resources that libpng allocated
    png_destroy_write_struct(&png_ptr, &info_ptr);
    return EXIT_SUCCESS;
}

//Function Definition
void set_image_data(png_bytep * data, int w, int h)
{
    for (int y = 0; y < h; y++)
    {
        for (int x = 0; x < w; x++)
        {
            //Calculate index for the current pixel
            int index = (y * w + x) * 3;
            printf("x: %d, y: %d, w: %d, h: %d, index: %d\n", x, y, w, h, index);
            //Make sure the indices are within bounds
            if (y < h && x < w && index < 3 * w * h)
            {
                //Now set RGB values for pixel in hexadecimal color (#FFAABB)
                data[y][index] = (int)(0xFF * 0xFF / 255); //Red
                data[y][index + 1] = (int)(0xAA * 0xFF / 255); //Green
                data[y][index + 2] = (int)(0xBB * 0xFF / 255); //Blue
            }
            else
            {
                fprintf(stderr, "Attempted to access out-of-bounds memory\n");
                exit(EXIT_FAILURE);
            }
        }
    }
}