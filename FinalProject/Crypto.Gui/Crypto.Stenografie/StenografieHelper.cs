using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypto.Stenografie
{
    class StenografieHelper
    {
        public enum Opdracht
        {
            Verbergen,
            Vullen_met_nullen
        };

        /// <summary>
        /// TODO: een regel tekst verbergen in een afbeelding dmv een kleine aanpassing van pixels
        /// </summary>
        /// <param name="text"></param>
        /// <param name="afbeelding"></param>
        /// <returns></returns>
        public static Bitmap embedText(string text, Bitmap afbeelding)
        {
            Opdracht opdracht = Opdracht.Verbergen;
            int letterPositie = 0;
            int letterWaarde = 0;
            long pixelPositie = 0;
            int nullen = 0;
            int R = 0, G = 0, B = 0;

            // Voor de hele afbeelding hoogte
            for (int i = 0; i < afbeelding.Height; i++)
            {
                // Voor de hele afbeelding breedte
                for (int j = 0; j < afbeelding.Width; j++)
                {
                    // De kleur van de pixel van een afbeelding toekennen aan een lokale var "pixel"
                    Color pixel = afbeelding.GetPixel(j, i);

                    // TODO: Uitzoeken hoe dit weer werkt
                    R = pixel.R - pixel.R % 2;
                    G = pixel.G - pixel.G % 2;
                    B = pixel.B - pixel.B % 2;

                    // Elke kleur (RGB) bestaad uit 3 getallen dus deze doorlopen we
                    for (int n = 0; n < 3; n++)
                    {
                        // Bij de 8ste positie
                        if (pixelPositie % 8 == 0)
                        {
                            // als de opdracht is "vullen_met_nullen"
                            // TODO: nullen == 8 ???
                            if (opdracht == Opdracht.Vullen_met_nullen && nullen == 8)
                            {
                                // TODO: Als de rest van (de pixel)/3 kleiner is dan 2 --> 0 1 2 3 4 5 6 (7)
                                if ((pixelPositie - 1) % 3 < 2)
                                {
                                    // verander de kleur van de pixel op positie
                                    afbeelding.SetPixel(j, i, Color.FromArgb(R, G, B));
                                }
                                return afbeelding;
                            }

                            // Wanneer de letterPositie buiten de text-lengte gaat (er is nog afbeelding "over" maar geen tekst om er in te verbergen
                            if (letterPositie >= text.Length)
                            {
                                // Vul de rest van de afbeelding (pixels kleuren) met nullen
                                opdracht = Opdracht.Vullen_met_nullen;
                            }
                            else
                            {
                                // neem de letterwaarde van de letter op positie
                                letterWaarde = text[letterPositie++];
                            }
                        }

                        // De rest van de pixelpositie -> word opgevangen in een int dus geen kommagetallen
                        switch (pixelPositie % 3)
                        {
                            // in geval van R
                            case 0:
                                {
                                    if (opdracht == Opdracht.Verbergen)
                                    {
                                        // the rightmost bit in the character will be (charValue % 2)
                                        // to put this value instead of the LSB of the pixel element
                                        // just add it to it
                                        // recall that the LSB of the pixel element had been cleared
                                        // before this operation
                                        R += letterWaarde % 2;

                                        // removes the added rightmost bit of the character
                                        // such that next time we can reach the next one
                                        letterWaarde /= 2;
                                    }
                                }
                                break;
                            // in geval van G
                            case 1:
                                {
                                    if (opdracht == Opdracht.Verbergen)
                                    {
                                        G += letterWaarde % 2;

                                        letterWaarde /= 2;
                                    }
                                }
                                break;
                            // in geval van B
                            case 2:
                                {
                                    if (opdracht == Opdracht.Verbergen)
                                    {
                                        B += letterWaarde % 2;

                                        letterWaarde /= 2;
                                    }

                                    afbeelding.SetPixel(j, i, Color.FromArgb(R, G, B));
                                }
                                break;
                        }

                        pixelPositie++;

                        if (opdracht == Opdracht.Vullen_met_nullen)
                        {
                            // increment the value of zeros until it is 8
                            nullen++;
                        }
                    }
                }
            }
            return afbeelding;
        }

        /// <summary>
        /// TODO: een regel tekst uit een afbeelding halen dmv de kleine aapassingen in pixels te vinden
        /// </summary>
        /// <param name="bmp"></param>
        /// <returns></returns>
        public static string extractText(Bitmap bmp)
        {
            int colorUnitIndex = 0;
            int charValue = 0;

            // holds the text that will be extracted from the image
            string extractedText = String.Empty;

            // pass through the rows
            for (int i = 0; i < bmp.Height; i++)
            {
                // pass through each row
                for (int j = 0; j < bmp.Width; j++)
                {
                    Color pixel = bmp.GetPixel(j, i);

                    // for each pixel, pass through its elements (RGB)
                    for (int n = 0; n < 3; n++)
                    {
                        switch (colorUnitIndex % 3)
                        {
                            case 0:
                                {
                                    // get the LSB from the pixel element (will be pixel.R % 2)
                                    // then add one bit to the right of the current character
                                    // this can be done by (charValue = charValue * 2)
                                    // replace the added bit (which value is by default 0) with
                                    // the LSB of the pixel element, simply by addition
                                    charValue = charValue * 2 + pixel.R % 2;
                                }
                                break;
                            case 1:
                                {
                                    charValue = charValue * 2 + pixel.G % 2;
                                }
                                break;
                            case 2:
                                {
                                    charValue = charValue * 2 + pixel.B % 2;
                                }
                                break;
                        }

                        colorUnitIndex++;

                        // if 8 bits has been added, then add the current character to the result text
                        if (colorUnitIndex % 8 == 0)
                        {
                            // reverse? of course, since each time the process happens on the right (for simplicity)
                            charValue = reverseBits(charValue);

                            // can only be 0 if it is the stop character (the 8 zeros)
                            if (charValue == 0)
                            {
                                return extractedText;
                            }

                            // convert the character value from int to char
                            char c = (char)charValue;

                            // add the current character to the result text
                            extractedText += c.ToString();
                        }
                    }
                }
            }

            return extractedText;
        }

        /// <summary>
        /// TODO: nullen invullen waar de data is uitgehaald
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public static int reverseBits(int n)
        {
            int result = 0;

            for (int i = 0; i < 8; i++)
            {
                result = result * 2 + n % 2;

                n /= 2;
            }

            return result;
        }
    }
}
