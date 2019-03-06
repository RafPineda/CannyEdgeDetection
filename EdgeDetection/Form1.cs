//Made by: Rafael Pineda
//Comments: Canney Edge Detection algorithm.
//This program takes a user input of a graphic image.
//The user can then choose to apply a Gaussian Mask (smoothing operation) or opt to skip the mask and attempt edge detection.
//The edge detection algorithm works by selecting points in the image with local maximal gradients. Then those points will be added to a queue, where they will check if any nearby points are eligible for entering the queue also.
//The user can change the minimum and maximum thresholds, thereby altering the amount of noise allowed into the edge detection. 


using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Imaging;

namespace EdgeDetection
{
    public partial class CanneyEdge : Form
    {
        int lowThld = 20, highThld = 30;  //low and high thresholds used for deciding which "edge" points are true


        public struct luma //a structure to hold the direction of the gradiant
        {
            public int x;
            public int y;
            public int manhattanNorm;
        }


        Bitmap FullBitmap = new Bitmap(1, 1);
        Bitmap FullBitmapCopy = new Bitmap(1, 1); //reserved bitamp for repeated smoothing operations
        Bitmap OriginalImage = new Bitmap(1, 1);
        Bitmap imageInstance = new Bitmap(1, 1);
        Bitmap edgesFound = new Bitmap(1, 1);

        public CanneyEdge()
        {
            InitializeComponent();
            smoothButton.Enabled = false;
            Edge_Find_Button.Enabled = false;
        }

        private void load_Image_Click(object sender, EventArgs e) //load image into program
        {
            OpenFileDialog imagefileopen = new OpenFileDialog();
            imagefileopen.Filter = "Image Files(*.jpg;*jpeg; *.gif; *.bmp; *.png) | *.jpg; *.jpeg; *.gif; *.bmp; *.png";
            if (imagefileopen.ShowDialog() == DialogResult.OK) //ask for user image
            {
                pictureBox1.Image = new Bitmap(imagefileopen.FileName);
                pictureBox1.Size = pictureBox1.Image.Size;
                Refresh();

                smoothButton.Enabled = true;
                Edge_Find_Button.Enabled = true;
                FullBitmap = new Bitmap(pictureBox1.Image.Width + 4, pictureBox1.Image.Height + 4, PixelFormat.Format24bppRgb); //make a new bitmap with equal dimensions as original image

                OriginalImage = (Bitmap)pictureBox1.Image;
                edgesFound = (Bitmap)pictureBox1.Image;
                FullBitmap = makeFullBitmap(OriginalImage, FullBitmap);  //add top, bottom, left and right rows
                FullBitmapCopy = new Bitmap(FullBitmap);
            }
        }

        public void smoothButton_Click(object sender, EventArgs e) //on push, smoothbutton will smooth the image and display it in a new form
        {
            smoothButton.Text = "Smoothing...";
            Edge_Find_Button.Text = "Smoothing...";
            smoothButton.Enabled = false;
            Edge_Find_Button.Enabled = false;

            load_Image.Enabled = false;
            Cursor.Current = Cursors.WaitCursor;
            imageInstance = new Bitmap(pictureBox1.Image.Width, pictureBox1.Image.Height);
            smoothFxn(OriginalImage, FullBitmap); //smooth the original image, storing that smoothed version in "Image Instance" by using the equation 1/237 * all 25 bits


            //create new window, set the picture in new window to the smoothed "Image Instance"

            Form Smoothed = new Smoothed();
            if (imageInstance != null)
            {
                PictureBox tempPicture = new PictureBox();
                tempPicture.Size = imageInstance.Size;
                Smoothed.Controls.Add(tempPicture);
                tempPicture.Image = imageInstance;
                Smoothed.Show();
            }



            pictureBox1.Size = pictureBox1.Image.Size;
            FullBitmap = new Bitmap(pictureBox1.Image.Width + 4, pictureBox1.Image.Height + 4, PixelFormat.Format24bppRgb); //make a new bitmap with equal dimensions as original image
            FullBitmap = makeFullBitmap(imageInstance, FullBitmap);  //add top, bottom, left and right rows

            Cursor.Current = Cursors.Default;
            smoothButton.Text = "Smooth";
            Edge_Find_Button.Text = "DetectEdge";
            Edge_Find_Button.Enabled = true;
            smoothButton.Enabled = true;
            load_Image.Enabled = true;
        }
        private void smoothFxn(Bitmap ourPicture, Bitmap referenceBMP)  //"ourPicture" is the ORIGINAL image
                                                                        //referenceBMP is a bitmap containing 4 rows and 4 columns, so that we can perform smoothing averages by dividing by 
                                                                        //resulting bitmap is stored in "imageInstance"
        {
            int H = ourPicture.Height;
            int W = ourPicture.Width;
            int smoothRed = 0, smoothBlue = 0, smoothGreen = 0;
            int multiplier = 1;
            Color sampledColor = new Color();

            int[,] redValues = new int[FullBitmapCopy.Width, FullBitmapCopy.Height];
            int[,] greenValues = new int[FullBitmapCopy.Width, FullBitmapCopy.Height];
            int[,] blueValues = new int[FullBitmapCopy.Width, FullBitmapCopy.Height];

            //three arrays, one for red components, blue components and green components
            for (int i = 0; i < FullBitmapCopy.Width; i++)
            {
                for (int j = 0; j < FullBitmapCopy.Height; j++)
                {
                    sampledColor = FullBitmapCopy.GetPixel(i, j);
                    redValues[i, j] = sampledColor.R;
                    greenValues[i, j] = sampledColor.G;
                    blueValues[i, j] = sampledColor.B;
                }

            }

            //calculate the grayscale values
            for (int j = 2; j < H + 2; j++)
                for (int i = 2; i < W + 2; i++)
                {



                    //Column 1
                    multiplier = 1;
                    smoothRed += multiplier * redValues[i - 2, j - 2];
                    smoothGreen += multiplier * greenValues[i - 2, j - 2];
                    smoothBlue += multiplier * blueValues[i - 2, j - 2];

                    multiplier = 4;
                    smoothRed += multiplier * redValues[i - 2, j - 1];
                    smoothGreen += multiplier * greenValues[i - 2, j - 1];
                    smoothBlue += multiplier * blueValues[i - 2, j - 1];

                    multiplier = 7;
                    smoothRed += multiplier * redValues[i - 2, j];
                    smoothGreen += multiplier * greenValues[i - 2, j];
                    smoothBlue += multiplier * blueValues[i - 2, j];

                    multiplier = 4;
                    smoothRed += multiplier * redValues[i - 2, j + 1];
                    smoothGreen += multiplier * greenValues[i - 2, j + 1];
                    smoothBlue += multiplier * blueValues[i - 2, j + 1];

                    multiplier = 1;
                    smoothRed += multiplier * redValues[i - 2, j + 2];
                    smoothGreen += multiplier * greenValues[i - 2, j + 2];
                    smoothBlue += multiplier * blueValues[i - 2, j + 2];

                    /***************/

                    //column 2

                    multiplier = 4;
                    smoothRed += multiplier * redValues[i - 1, j - 2];
                    smoothGreen += multiplier * greenValues[i - 1, j - 2];
                    smoothBlue += multiplier * blueValues[i - 1, j - 2];

                    multiplier = 16;
                    smoothRed += multiplier * redValues[i - 1, j - 1];
                    smoothGreen += multiplier * greenValues[i - 1, j - 1];
                    smoothBlue += multiplier * blueValues[i - 1, j - 1];

                    multiplier = 26;
                    smoothRed += multiplier * redValues[i - 1, j];
                    smoothGreen += multiplier * greenValues[i - 1, j];
                    smoothBlue += multiplier * blueValues[i - 1, j];

                    multiplier = 16;
                    smoothRed += multiplier * redValues[i - 1, j + 1];
                    smoothGreen += multiplier * greenValues[i - 1, j + 1];
                    smoothBlue += multiplier * blueValues[i - 1, j + 1];

                    multiplier = 4;
                    smoothRed += multiplier * redValues[i - 1, j + 2];
                    smoothGreen += multiplier * greenValues[i - 1, j + 2];
                    smoothBlue += multiplier * blueValues[i - 1, j + 2];

                    /***************/

                    //column 3
                    multiplier = 7;
                    smoothRed += multiplier * redValues[i, j - 2];
                    smoothGreen += multiplier * greenValues[i, j - 2];
                    smoothBlue += multiplier * blueValues[i, j - 2];

                    multiplier = 26;
                    sampledColor = referenceBMP.GetPixel(i, j - 1);
                    smoothRed += multiplier * redValues[i, j - 1];
                    smoothGreen += multiplier * greenValues[i, j - 1];
                    smoothBlue += multiplier * blueValues[i, j - 1];

                    multiplier = 41;
                    smoothRed += multiplier * redValues[i, j];
                    smoothGreen += multiplier * greenValues[i, j];
                    smoothBlue += multiplier * blueValues[i, j];

                    multiplier = 26;
                    smoothRed += multiplier * redValues[i, j + 1];
                    smoothGreen += multiplier * greenValues[i, j + 1];
                    smoothBlue += multiplier * blueValues[i, j + 1];


                    multiplier = 7;
                    smoothRed += multiplier * redValues[i, j + 2];
                    smoothGreen += multiplier * greenValues[i, j + 2];
                    smoothBlue += multiplier * blueValues[i, j + 2];

                    /***************/

                    //column 4

                    multiplier = 4;
                    smoothRed += multiplier * redValues[i + 1, j - 2];
                    smoothGreen += multiplier * greenValues[i + 1, j - 2];
                    smoothBlue += multiplier * blueValues[i + 1, j - 2];

                    multiplier = 16;
                    smoothRed += multiplier * redValues[i + 1, j - 1];
                    smoothGreen += multiplier * greenValues[i + 1, j - 1];
                    smoothBlue += multiplier * blueValues[i + 1, j - 1];

                    multiplier = 26;
                    smoothRed += multiplier * redValues[i + 1, j];
                    smoothGreen += multiplier * greenValues[i + 1, j];
                    smoothBlue += multiplier * blueValues[i + 1, j];

                    multiplier = 16;
                    smoothRed += multiplier * redValues[i + 1, j + 1];
                    smoothGreen += multiplier * greenValues[i + 1, j + 1];
                    smoothBlue += multiplier * blueValues[i + 1, j + 1];

                    multiplier = 4;
                    smoothRed += multiplier * redValues[i + 1, j + 2];
                    smoothGreen += multiplier * greenValues[i + 1, j + 2];
                    smoothBlue += multiplier * blueValues[i + 1, j + 2];


                    /***************/

                    //column 5 

                    multiplier = 1;
                    smoothRed += multiplier * redValues[i + 2, j - 2];
                    smoothGreen += multiplier * greenValues[i + 2, j - 2];
                    smoothBlue += multiplier * blueValues[i + 2, j - 2];

                    multiplier = 4;
                    smoothRed += multiplier * redValues[i + 2, j - 1];
                    smoothGreen += multiplier * greenValues[i + 2, j - 1];
                    smoothBlue += multiplier * blueValues[i + 2, j - 1];

                    multiplier = 7;
                    smoothRed += multiplier * redValues[i + 2, j];
                    smoothGreen += multiplier * greenValues[i + 2, j];
                    smoothBlue += multiplier * blueValues[i + 2, j];

                    multiplier = 4;
                    smoothRed += multiplier * redValues[i + 2, j + 1];
                    smoothGreen += multiplier * greenValues[i + 2, j + 1];
                    smoothBlue += multiplier * blueValues[i + 2, j + 1];

                    multiplier = 1;
                    smoothRed += multiplier * redValues[i + 2, j + 2];
                    smoothGreen += multiplier * greenValues[i + 2, j + 2];
                    smoothBlue += multiplier * blueValues[i + 2, j + 2];



                    sampledColor = Color.FromArgb(smoothRed / 273, smoothGreen / 273, smoothBlue / 273);


                    imageInstance.SetPixel(i - 2, j - 2, sampledColor);
                    smoothRed = 0;
                    smoothBlue = 0;
                    smoothGreen = 0;
                }

        }


        /*
         makeOriginalFullBitmap creates a new bitmap with 2 and columns of reflected bits on the top, bottom, left and right sides
         Input: reference bitmap "ourPicture", target bitmap BMP
         Process: Finds pixels from "ourPicture" and pastes them in target BMP in the appropriate location
         Returns: BMP
         * */
        private Bitmap makeFullBitmap(Bitmap ourPicture, Bitmap BMP)
        {


            int Width = ourPicture.Width;
            int Height = ourPicture.Height;
            float varx = ourPicture.HorizontalResolution;
            float vary = ourPicture.VerticalResolution;
            BMP.SetResolution(varx, vary);

            /*** set top left corner ****/

            /*  00 | 10
                01 | 11
                */


            Color sampledColor = ourPicture.GetPixel(1, 1); // 00
            BMP.SetPixel(0, 0, sampledColor);

            sampledColor = ourPicture.GetPixel(0, 1);       // 10
            BMP.SetPixel(1, 0, sampledColor);

            sampledColor = ourPicture.GetPixel(1, 0);       // 01
            BMP.SetPixel(0, 1, sampledColor);

            sampledColor = ourPicture.GetPixel(0, 0);       // 11
            BMP.SetPixel(1, 1, sampledColor);

            /*** set bottom left corner ****/
            /*
                0, Height           |       1, Height
                0, Height + 1       |       1, Height + 1

             */

            sampledColor = ourPicture.GetPixel(1, Height - 1); //0, H
            BMP.SetPixel(0, Height + 2, sampledColor);

            sampledColor = ourPicture.GetPixel(0, Height - 1); //1, H
            BMP.SetPixel(1, Height + 2, sampledColor);

            sampledColor = ourPicture.GetPixel(1, Height - 2); //0, H + 1
            BMP.SetPixel(0, Height + 3, sampledColor);

            sampledColor = ourPicture.GetPixel(0, Height - 2);
            BMP.SetPixel(1, Height + 3, sampledColor);


            /*** set top right corner ****/

            /*  Width, 0        |       Width+1, 0
                Width, 1        |       Width+1, 1            
             */

            sampledColor = ourPicture.GetPixel(Width - 1, 1); // W 0
            BMP.SetPixel(Width + 2, 0, sampledColor);

            sampledColor = ourPicture.GetPixel(Width - 2, 1); // W1 0
            BMP.SetPixel(Width + 3, 0, sampledColor);

            sampledColor = ourPicture.GetPixel(Width - 1, 0); // W 1
            BMP.SetPixel(Width + 2, 1, sampledColor);

            sampledColor = ourPicture.GetPixel(Width - 2, 0); // W1 1
            BMP.SetPixel(Width + 3, 1, sampledColor);

            /*** set borrom right corner ****/
            /* Width, Height        | Width + 1, Height
               Width, Height + 1    | Width + 1, Height + 1              
             */
            sampledColor = ourPicture.GetPixel(Width - 1, Height - 1); // W H
            BMP.SetPixel(Width + 2, Height + 2, sampledColor);

            sampledColor = ourPicture.GetPixel(Width - 2, Height - 1); //W1 H
            BMP.SetPixel(Width + 3, Height + 2, sampledColor);

            sampledColor = ourPicture.GetPixel(Width - 1, Height - 2); //W H1
            BMP.SetPixel(Width + 2, Height + 3, sampledColor);

            sampledColor = ourPicture.GetPixel(Width - 2, Height - 2); //W1 H1
            BMP.SetPixel(Width + 3, Height + 3, sampledColor);

            for (int i = 0; i < Width; i++) // sets row mirrors (1st/2nd rows and 2nd to last/last rows)
            {
                sampledColor = ourPicture.GetPixel(i, 1); //rows
                BMP.SetPixel(i + 2, 0, sampledColor);
                sampledColor = ourPicture.GetPixel(i, 0);
                BMP.SetPixel(i + 2, 1, sampledColor);

                sampledColor = ourPicture.GetPixel(i, Height - 2);
                BMP.SetPixel(i + 2, Height + 3, sampledColor);
                sampledColor = ourPicture.GetPixel(i, Height - 1);
                BMP.SetPixel(i + 2, Height + 2, sampledColor);
            }
            for (int i = 0; i < Height; i++) // columns
            {
                sampledColor = ourPicture.GetPixel(0, i);
                BMP.SetPixel(1, i + 2, sampledColor);
                sampledColor = ourPicture.GetPixel(1, i);
                BMP.SetPixel(0, i + 2, sampledColor);

                sampledColor = ourPicture.GetPixel(Width - 1, i);
                BMP.SetPixel(Width + 2, i + 2, sampledColor);
                sampledColor = ourPicture.GetPixel(Width - 2, i);
                BMP.SetPixel(Width + 3, i + 2, sampledColor);
            }




            Graphics g = Graphics.FromImage(BMP);
            g.DrawImage(ourPicture, 2, 2); //paste "ourPicture" into location (2,2) of BMP
            g.Dispose();
            return BMP;

        }



        private void makeGray()
        {

            for (int i = 0; i < FullBitmap.Width; i++)
            {
                for (int j = 0; j < FullBitmap.Height; j++)
                {
                    //get the pixels from original image
                    Color originalColor = FullBitmap.GetPixel(i, j);

                    //create a gray scale copy
                    int grayScaled = (int)((originalColor.R * 0.3) + (originalColor.G * 0.59) + (originalColor.B * 0.11));

                    //use grayscale copy to create a new image, pixel by pixel

                    Color newColor = Color.FromArgb(grayScaled, grayScaled, grayScaled);
                    FullBitmap.SetPixel(i, j, newColor);
                }


            }



        }

        private void setThld_Click(object sender, EventArgs e)
        {
            int tempa = lowThld, tempb = highThld;
            if (!int.TryParse(LowValue.Text, out lowThld))
                lowThld = tempa;
            if (!int.TryParse(HighValue.Text, out highThld))
                highThld = tempb;

            if (lowThld < 0)
                lowThld = 0;
            if (highThld < 0)
                highThld = 0;
            if (lowThld > highThld)
                lowThld = highThld;
            LowValue.Text = lowThld.ToString();
            HighValue.Text = highThld.ToString();

        }

        private void Edge_Find_Button_Click(object sender, EventArgs e)
        {

            //create grayscale copy of extended array
            makeGray();
            Graphics g = Graphics.FromImage(edgesFound); //set our image instance to white
            g.Clear(Color.Black);
            int[,] grayValues = new int[FullBitmap.Width, FullBitmap.Height];

            luma[,] gradiantValues = new luma[FullBitmap.Width - 2, FullBitmap.Height - 2];

            byte[,] map = new byte[edgesFound.Width, edgesFound.Height];
            /* array initialized to 0. The purpose of this array is to store which locations are marked as gray zones, which can be "powered up" to full black 
             * */
            int average1, average2; //calculation of the intersections of the gradiant and nearby colors
            Queue<Point> listOfPoints = new Queue<Point>();

            //move gray values into an array so it's easier to access
            for (int i = 0; i < FullBitmap.Width; i++)
            {
                for (int j = 0; j < FullBitmap.Height; j++)
                {
                    grayValues[i, j] = FullBitmap.GetPixel(i, j).R; //since it's grayscale, red, green and blue components are all the same                    
                }

            }

            //calculate x and y gradient                                                       
            for (int i = 2; i < FullBitmap.Width - 2; i++)
            {
                for (int j = 2; j < FullBitmap.Height - 2; j++)
                {
                    int x = (grayValues[i, j - 1] - grayValues[i - 2, j - 1]) / 2;
                    gradiantValues[i - 2, j - 2].x = x;
                    int y = (grayValues[i - 1, j] - grayValues[i - 1, j - 2]) / 2;
                    gradiantValues[i - 2, j - 2].y = y;
                    gradiantValues[i - 2, j - 2].manhattanNorm = Math.Abs(x) + Math.Abs(y);
                    map[i - 2, j - 2] = 0; //while we're here, initialize all our map bits to 0

                }
            }

            // find which pixels are for sure edges, and greater than High threshold, add to queue
            //pixels that might be edges, check if they are above low threshold, otherwise set to 0
            for (int i = 3; i < FullBitmap.Width - 3; i++)
            {
                for (int j = 3; j < FullBitmap.Height - 3; j++)
                {
                    int CurrentValue = gradiantValues[i - 2, j - 2].manhattanNorm;
                    //if x is positive, we're on right-side coordinate planes
                    if (gradiantValues[i - 2, j - 2].x > 0)
                    {
                        //if y is positive, we're above x axis
                        if (gradiantValues[i - 2, j - 2].y > 0)
                        {
                            //slope where x increases faster than y, since we are in the 1st quadrent, we look at the average from 1,1 to 1,0 and -1,0 to -1,-1
                            if (Math.Abs(gradiantValues[i - 2, j - 2].x) > Math.Abs(gradiantValues[i - 2, j - 2].y))
                            {
                                average1 = (gradiantValues[i + 1, j + 1].manhattanNorm + gradiantValues[i + 1, j].manhattanNorm);
                                average2 = (gradiantValues[i - 1, j - 1].manhattanNorm + gradiantValues[i - 1, j].manhattanNorm);
                                if (CurrentValue > average1 && CurrentValue > average2) //if our gray value is higher than both, it's for sure a black point, add to list
                                {
                                    if (CurrentValue > highThld)
                                        listOfPoints.Enqueue(new Point(i - 2, j - 2)); //add our current coordinates to a list. later, we'll see if we can "power up" any nearby gray spots
                                }

                                else if ((CurrentValue > average1 || CurrentValue > average2) && CurrentValue > lowThld) //if our gray value is higher than only one, it might later be powered up
                                {

                                    map[i - 2, j - 2] = 1; //mark our map bit a 1, for grey value
                                }

                            }

                            //slope where y increases faster than x
                            else
                            {
                                average1 = (gradiantValues[i, j + 1].manhattanNorm + gradiantValues[i + 1, j + 1].manhattanNorm);
                                average2 = (gradiantValues[i - 1, j - 1].manhattanNorm + gradiantValues[i, j - 1].manhattanNorm);
                                if (CurrentValue > average1 && CurrentValue > average2) //if our gray value is higher than both, it's for sure a black point, add to list
                                {
                                    if (CurrentValue > highThld)
                                        listOfPoints.Enqueue(new Point(i - 2, j - 2)); //add our current coordinates to a list. later, we'll see if we can "power up" any nearby gray spots
                                }

                                else if ((CurrentValue > average1 || CurrentValue > average2) && CurrentValue > lowThld) //if our gray value is higher than only one, it might later be powered up
                                {
                                    map[i - 2, j - 2] = 1; //mark our map bit a 1, for grey value
                                }
                            }

                        }

                        //otherwise, y is negative, we're below y axis
                        else
                        {
                            if (Math.Abs(gradiantValues[i - 2, j - 2].x) > Math.Abs(gradiantValues[i - 2, j - 2].y))
                            {
                                average1 = (gradiantValues[i - 1, j + 1].manhattanNorm + gradiantValues[i - 1, j].manhattanNorm);
                                average2 = (gradiantValues[i + 1, j - 1].manhattanNorm + gradiantValues[i + 1, j].manhattanNorm);
                                if (CurrentValue > average1 && CurrentValue > average2) //if our gray value is higher than both, it's for sure a black point, add to list
                                {
                                    if (CurrentValue > highThld)
                                        listOfPoints.Enqueue(new Point(i - 2, j - 2)); //add our current coordinates to a list. later, we'll see if we can "power up" any nearby gray spots
                                }

                                else if ((CurrentValue > average1 || CurrentValue > average2) && CurrentValue > lowThld) //if our gray value is higher than only one, it might later be powered up
                                {
                                    map[i - 2, j - 2] = 1; //mark our map bit a 1, for grey value
                                }

                            }


                            else
                            {
                                average1 = (gradiantValues[i, j - 1].manhattanNorm + gradiantValues[i + 1, j - 1].manhattanNorm);
                                average2 = (gradiantValues[i - 1, j + 1].manhattanNorm + gradiantValues[i, j + 1].manhattanNorm);
                                if (CurrentValue > average1 && CurrentValue > average2) //if our gray value is higher than both, it's for sure a black point, add to list
                                {
                                    if (CurrentValue > highThld)
                                        listOfPoints.Enqueue(new Point(i - 2, j - 2)); //add our current coordinates to a list. later, we'll see if we can "power up" any nearby gray spots
                                }

                                else if ((CurrentValue > average1 || CurrentValue > average2) && CurrentValue > lowThld) //if our gray value is higher than only one, it might later be powered up
                                {
                                    map[i - 2, j - 2] = 1; //mark our map bit a 1, for grey value
                                }

                            }


                        }

                    }



                    //otherwise, x is negative so we're on left coordinate planes
                    else
                    {
                        if (gradiantValues[i - 2, j - 2].y > 0)
                        {
                            //slope where x increases faster than y
                            if (Math.Abs(gradiantValues[i - 2, j - 2].x) > Math.Abs(gradiantValues[i - 2, j - 2].y))
                            {
                                average1 = (gradiantValues[i - 1, j + 0].manhattanNorm + gradiantValues[i - 1, j + 1].manhattanNorm);
                                average2 = (gradiantValues[i + 1, j].manhattanNorm + gradiantValues[i + 1, j - 1].manhattanNorm);
                                if (CurrentValue > average1 && CurrentValue > average2) //if our gray value is higher than both, it's for sure a black point, add to list
                                {
                                    if (CurrentValue > highThld)
                                        listOfPoints.Enqueue(new Point(i - 2, j - 2)); //add our current coordinates to a list. later, we'll see if we can "power up" any nearby gray spots
                                }

                                else if ((CurrentValue > average1 || CurrentValue > average2) && CurrentValue > lowThld) //if our gray value is higher than only one, it might later be powered up
                                {
                                    map[i - 2, j - 2] = 1; //mark our map bit a 1, for grey value
                                }

                            }

                            //slope where y increases faster than x
                            else
                            {
                                average1 = (gradiantValues[i - 1, j + 1].manhattanNorm + gradiantValues[i, j + 1].manhattanNorm);
                                average2 = (gradiantValues[i, j - 1].manhattanNorm + gradiantValues[i + 1, j - 1].manhattanNorm);
                                if (CurrentValue > average1 && CurrentValue > average2) //if our gray value is higher than both, it's for sure a black point, add to list
                                {
                                    if (CurrentValue > highThld)
                                        listOfPoints.Enqueue(new Point(i - 2, j - 2)); //add our current coordinates to a list. later, we'll see if we can "power up" any nearby gray spots
                                }

                                else if ((CurrentValue > average1 || CurrentValue > average2) && CurrentValue > lowThld) //if our gray value is higher than only one, it might later be powered up
                                {
                                    map[i - 2, j - 2] = 1; //mark our map bit a 1, for grey value
                                }
                            }
                        }

                        else // in quadrent 3
                        {
                            if (Math.Abs(gradiantValues[i - 2, j - 2].x) > Math.Abs(gradiantValues[i - 2, j - 2].y))
                            {
                                average1 = (gradiantValues[i - 1, j].manhattanNorm + gradiantValues[i - 1, j - 1].manhattanNorm);
                                average2 = (gradiantValues[i + 1, j + 1].manhattanNorm + gradiantValues[i + 1, j].manhattanNorm);
                                if (CurrentValue > average1 && CurrentValue > average2) //if our gray value is higher than both, it's for sure a black point, add to list
                                {
                                    if (CurrentValue > highThld)
                                        listOfPoints.Enqueue(new Point(i - 2, j - 2)); //add our current coordinates to a list. later, we'll see if we can "power up" any nearby gray spots
                                }

                                else if ((CurrentValue > average1 || CurrentValue > average2) && CurrentValue > lowThld) //if our gray value is higher than only one, it might later be powered up
                                {
                                    map[i - 2, j - 2] = 1; //mark our map bit a 1, for grey value
                                }

                            }


                            else
                            {
                                average1 = (gradiantValues[i - 1, j - 1].manhattanNorm + gradiantValues[i, j - 1].manhattanNorm);
                                average2 = (gradiantValues[i, j + 1].manhattanNorm + gradiantValues[i + 1, j + 1].manhattanNorm);
                                if (CurrentValue > average1 && CurrentValue > average2) //if our gray value is higher than both, it's for sure a black point, add to list
                                {
                                    if (CurrentValue > highThld)
                                        listOfPoints.Enqueue(new Point(i - 2, j - 2)); //add our current coordinates to a list. later, we'll see if we can "power up" any nearby gray spots
                                }

                                else if ((CurrentValue > average1 || CurrentValue > average2) && CurrentValue > lowThld) //if our gray value is higher than only one, it might later be powered up
                                {
                                    map[i - 2, j - 2] = 1; //mark our map bit a 1, for grey value
                                }
                            }
                        }








                    }
                }
            }


            //for each black point, set a black pixel
            //power up near by pixels
            while (listOfPoints.Count > 0)
            {
                Point currentPoint = listOfPoints.Dequeue();
                edgesFound.SetPixel(currentPoint.X, currentPoint.Y, Color.White);
                //check any nearby points. Enqueue them if they're gray (== 1 on map array)

                try
                {
                    if (map[currentPoint.X - 1, currentPoint.Y - 1] == 1)
                    {
                        map[currentPoint.X - 1, currentPoint.Y - 1] = 0;
                        listOfPoints.Enqueue(new Point(currentPoint.X - 1, currentPoint.Y - 1));

                    }
                }
                catch (System.IndexOutOfRangeException)
                {
                    //if we catch any outofrange exceptions, we simply do nothing. we're at the border of the picture, there are no edges off the border

                }
                try
                {
                    if (map[currentPoint.X, currentPoint.Y - 1] == 1)
                    {
                        map[currentPoint.X, currentPoint.Y - 1] = 0;
                        listOfPoints.Enqueue(new Point(currentPoint.X, currentPoint.Y - 1));

                    }
                }
                catch (System.IndexOutOfRangeException)
                {

                }
                try
                {
                    if (map[currentPoint.X + 1, currentPoint.Y - 1] == 1)
                    {
                        map[currentPoint.X + 1, currentPoint.Y - 1] = 0;
                        listOfPoints.Enqueue(new Point(currentPoint.X + 1, currentPoint.Y - 1));

                    }
                }
                catch (System.IndexOutOfRangeException)
                {

                }


                try
                {
                    if (map[currentPoint.X - 1, currentPoint.Y] == 1)
                    {
                        map[currentPoint.X - 1, currentPoint.Y] = 0;
                        listOfPoints.Enqueue(new Point(currentPoint.X - 1, currentPoint.Y));

                    }
                }
                catch (System.IndexOutOfRangeException)
                {

                }
                try
                {
                    if (map[currentPoint.X, currentPoint.Y] == 1)
                    {
                        map[currentPoint.X, currentPoint.Y] = 0;
                        listOfPoints.Enqueue(new Point(currentPoint.X, currentPoint.Y));

                    }
                }
                catch (System.IndexOutOfRangeException)
                {

                }

                try
                {
                    if (map[currentPoint.X + 1, currentPoint.Y] == 1)
                    {
                        map[currentPoint.X + 1, currentPoint.Y] = 0;
                        listOfPoints.Enqueue(new Point(currentPoint.X + 1, currentPoint.Y));

                    }
                }
                catch (System.IndexOutOfRangeException)
                {

                }

                try
                {
                    if (map[currentPoint.X - 1, currentPoint.Y + 1] == 1)
                    {
                        map[currentPoint.X - 1, currentPoint.Y + 1] = 0;
                        listOfPoints.Enqueue(new Point(currentPoint.X - 1, currentPoint.Y + 1));

                    }
                }
                catch (System.IndexOutOfRangeException)
                {

                }

                try
                {
                    if (map[currentPoint.X, currentPoint.Y + 1] == 1)
                    {
                        map[currentPoint.X, currentPoint.Y + 1] = 0;
                        listOfPoints.Enqueue(new Point(currentPoint.X, currentPoint.Y + 1));

                    }
                }
                catch (System.IndexOutOfRangeException)
                {

                }

                try
                {
                    if (map[currentPoint.X + 1, currentPoint.Y + 1] == 1)
                    {
                        map[currentPoint.X + 1, currentPoint.Y + 1] = 0;
                        listOfPoints.Enqueue(new Point(currentPoint.X + 1, currentPoint.Y + 1));

                    }
                }
                catch (System.IndexOutOfRangeException)
                {

                }



            }

            //open a new window showing the edges found
            Form Edges = new Edges();
            if (imageInstance != null)
            {
                PictureBox tempPicture = new PictureBox();
                tempPicture.Size = edgesFound.Size;
                Edges.Controls.Add(tempPicture);
                tempPicture.Image = edgesFound;
                Edges.Show();
            }
        }
    }
}
