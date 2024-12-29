import tkinter as tk
from tkinter import Button, Frame, Label, Entry
import requests
from PIL import Image, ImageTk
from io import BytesIO
from bs4 import BeautifulSoup

class BuildNewTool:
    def __init__(self, root):
        self.buildnewtoolwin = root
        self.buildnewtoolwin.geometry("700x700")
        self.buildnewtoolwin.title("Build New Tool")

        # Create a frame inside the root window
        self.frame = Frame(self.buildnewtoolwin, bg="#f0f0f0")
        self.frame.pack(padx=20, pady=20, fill='both', expand=True)

        # Add a button inside the frame
        buttonhoffmann = Button(self.frame, text="Hoffmann", command=self.printHoffmann)
        buttonhoffmann.grid(row=0, column=0, pady=20, padx=20)

    def printHoffmann(self):
        # Destroy the current frame
        self.frame.destroy()

        # Create a new frame
        self.frame = Frame(self.buildnewtoolwin, bg="#f0f0f0")
        self.frame.pack(padx=20, pady=20, fill='both', expand=True)

        # Create the entry and button widgets inside the new frame
        entry_label = Label(self.frame, text="Geben Sie die Artikelnummer ein (e.g., 308196):")
        entry_label.pack()

        entry = Entry(self.frame)
        entry.pack()

        label = Label(self.frame)
        label.pack()

        def fetch_image():
            url_value = entry.get()
            webpage_url = f"https://www.hoffmann-group.com/DE/de/hom/p/{url_value}"

            try:
                response = requests.get(webpage_url)
                soup = BeautifulSoup(response.text, 'html.parser')

                image_tags = soup.find_all('img')
                largest_image_url = None
                largest_width = 0
                largest_height = 0

                for img_tag in image_tags:
                    if 'src' in img_tag.attrs:
                        img_url = img_tag['src']
                        if not img_url.startswith("http"):
                            img_url = f"https://www.hoffmann-group.com{img_url}"

                        width = img_tag.get('width')
                        height = img_tag.get('height')

                        if width and height:
                            width = int(width)
                            height = int(height)

                            if width * height > largest_width * largest_height:
                                largest_image_url = img_url
                                largest_width = width
                                largest_height = height

                if largest_image_url:
                    response = requests.get(largest_image_url)
                    image_data = response.content
                    image = Image.open(BytesIO(image_data))
                    
                    # Resize the image to 250x250
                    image = image.resize((250, 250)).rotate(270, expand=True)


                    tk_image = ImageTk.PhotoImage(image)
                    label.config(image=tk_image)
                    label.image = tk_image
                else:
                    show_fallback_image()

            except requests.exceptions.RequestException as e:
                show_fallback_image()
                label.config(text=f"Error: {e}")

        def show_fallback_image():
            fallback_image_path = r"X:\Projekt\mtm\img\NOTFOUND.png"
            fallback_image = Image.open(fallback_image_path)
            fallback_image=fallback_image.resize((250, 250))
            tk_fallback_image = ImageTk.PhotoImage(fallback_image)
            label.config(image=tk_fallback_image)
            label.image = tk_fallback_image


        def backbutton():
            self.frame.destroy()
            BuildNewTool(self.buildnewtoolwin)

        # Create the buttons
        search_button = Button(self.frame, text="Search", command=fetch_image)
        search_button.grid(row=0)

        back_button = Button(self.frame, text="Back", command=backbutton)
        back_button.pack()

if __name__ == "__main__":
    root = tk.Tk()
    BuildNewTool(root)
    root.mainloop()
