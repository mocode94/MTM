import tkinter as tk
from tkinter import filedialog, messagebox
import fitz  # PyMuPDF
from PIL import Image, ImageTk
import io

class PDFSearcher:
    def __init__(self, root):
        self.root = root
        self.pdf_document = None
        self.search_results = []
        self.current_index = -1
        
        self.entry = tk.Entry(root, width=50)
        self.entry.pack(pady=10)

        self.search_button = tk.Button(root, text="Open PDF and Search", command=self.open_pdf)
        self.search_button.pack(pady=10)

        self.next_button = tk.Button(root, text="Next Result", command=self.show_next_result, state=tk.DISABLED)
        self.next_button.pack(pady=10)

        self.image_label = tk.Label(root)
        self.image_label.pack(pady=10)

        self.page_label = tk.Label(root, text="")
        self.page_label.pack(pady=5)

    def open_pdf(self):
        file_path = filedialog.askopenfilename(filetypes=[("PDF files", "*.pdf")])
        if file_path:
            self.search_text_in_pdf(file_path)

    def search_text_in_pdf(self, file_path):
        search_text = self.entry.get()  # Preserve spaces exactly as entered
        if not search_text:
            messagebox.showwarning("Input Error", "Please enter text to search.")
            return

        try:
            self.pdf_document = fitz.open(file_path)
        except Exception as e:
            messagebox.showerror("File Error", f"Failed to open PDF file: {e}")
            return

        self.search_results = []
        self.current_index = -1

        # Iterate through each page of the PDF and search for the text
        for page_num in range(self.pdf_document.page_count):
            page = self.pdf_document[page_num]
            text_instances = page.search_for(search_text)

            # Debug output to check the search instances found
            print(f"Page {page_num + 1}: found {len(text_instances)} instances of '{search_text}'")

            for inst in text_instances:
                self.search_results.append((page_num, inst))

        if not self.search_results:
            messagebox.showinfo("Not Found", f"Text '{search_text}' not found in the PDF file.")
        else:
            self.current_index = 0
            self.show_result(self.current_index)
            if len(self.search_results) > 1:
                self.next_button.config(state=tk.NORMAL)

    def show_result(self, index):
        page_num, _ = self.search_results[index]
        self.show_pdf_page(page_num)
        
    def show_next_result(self):
        self.current_index += 1
        if self.current_index < len(self.search_results):
            self.show_result(self.current_index)
        if self.current_index >= len(self.search_results) - 1:
            self.next_button.config(state=tk.DISABLED)

    def show_pdf_page(self, page_num):
        page = self.pdf_document.load_page(page_num)
        pix = page.get_pixmap()
        image = Image.open(io.BytesIO(pix.tobytes("png")))
        photo = ImageTk.PhotoImage(image)
        
        self.image_label.config(image=photo)
        self.image_label.image = photo
        self.page_label.config(text=f"Page {page_num + 1}")

# Create main window
root = tk.Tk()
root.title("PDF Searcher")

# Initialize the PDFSearcher class
pdf_searcher = PDFSearcher(root)

# Start the Tkinter event loop
root.mainloop()
