##################### Put all artikels from the catalog into the database ############################

import fitz  # PyMuPDF
import re
import sqlite3
from tkinter import Tk
from tkinter.filedialog import askopenfilename

# Open file dialog to select PDF
Tk().withdraw()  # Hide the root window
pdf_path = askopenfilename(title="Select PDF file", filetypes=[("PDF Files", "*.pdf")])

if pdf_path:
    # Regular expression pattern to match article numbers (letters and digits, at least 8 characters long)
    article_pattern = r'\b[A-Z0-9]{8,}\b'

    # Open the PDF
    pdf_document = fitz.open(pdf_path)

    # Create a set to hold unique article numbers
    article_numbers = set()

    # Loop through each page in the PDF
    for page_num in range(pdf_document.page_count):
        page = pdf_document.load_page(page_num)
        text = page.get_text()

        # Find all matches using the regular expression
        matches = re.findall(article_pattern, text)

        # Add matches to the set (to avoid duplicates)
        article_numbers.update(matches)

    # Print the unique article numbers found in the PDF
    print("Article Numbers found in the PDF:")
    for article in article_numbers:
        print(article)

    # Connect to the SQLite database (replace 'your_database.db' with the actual path)
    db_path = r"X:\Projekt\mtmgithub\MTM\data\MTMitems - Kopie.db"  # Replace with your actual DB path
    conn = sqlite3.connect(db_path)
    cursor = conn.cursor()

    # Ensure the Ingersoll table exists (replace with your actual table schema if needed)
    cursor.execute('''CREATE TABLE IF NOT EXISTS Ingersoll (art_No TEXT)''')

    # Insert the article numbers into the Ingersoll table
    for article in article_numbers:
        cursor.execute('INSERT INTO Ingersoll (art_No) VALUES (?)', (article,))

    # Commit the changes and close the connection
    conn.commit()
    conn.close()

    print(f"Inserted {len(article_numbers)} article numbers into the database.")

else:
    print("No PDF file selected.")













###################  THIS ONE WORKS  ############################
##################################################################
####################### pruduct number and product link ###########################


import re
import sqlite3
from urllib.parse import urljoin
from playwright.async_api import async_playwright
import asyncio

async def get_product_number(page_url):
    async with async_playwright() as p:
        browser = await p.chromium.launch(headless=True)  # Headless for faster execution
        page = await browser.new_page()

        # Block unnecessary resources (e.g., images, fonts) to speed up loading
        await page.route("**/*.{png,jpg,jpeg,css,woff,woff2,svg,ttf,eot}", lambda route: route.abort())

        # Set a short timeout for page navigation (5 seconds)
        try:
            await page.goto(page_url, wait_until="domcontentloaded", timeout=5000)  # 5 seconds timeout
        except:
            print(f"Timeout loading page: {page_url}")
            await browser.close()
            return None, None  # Return None for both product number and link

        # Extract the first product link from the search results
        product_link_locator = page.locator("a[href*='/catalogue/product/']").first
        product_url = await product_link_locator.get_attribute("href") if product_link_locator else None

        if not product_url:
            print(f"No product found for URL: {page_url}")
            await browser.close()
            return None, None

        # Ensure the URL is fully qualified (absolute URL)
        if product_url.startswith('/'):
            product_url = urljoin(page_url, product_url)  # Join with the search page URL to make it absolute

        # Fix the incorrect URL format
        product_url = product_url.replace("register/?back_url=/", "")

        # Extract the product number from the URL
        product_number_match = re.search(r'/catalogue/product/(\d+)', product_url)
        if product_number_match:
            product_number = product_number_match.group(1)
            print(f"Extracted product number: {product_number} from URL: {product_url}")
            await browser.close()
            return product_number, product_url

        await browser.close()
        return None, None

async def main():
    # Connect to the SQLite database
    db_path = r"X:\Projekt\mtmgithub\MTM\data\MTMitems - Kopie.db"  # Replace with your actual DB path
    conn = sqlite3.connect(db_path)
    cursor = conn.cursor()

    # Set the journal mode to WAL (Write-Ahead Logging) for immediate updates
    cursor.execute('PRAGMA journal_mode=WAL;')

    # # Fetch all article numbers (art_No) from the Ingersoll table
    # cursor.execute('SELECT art_No FROM Ingersoll')
    # art_numbers = cursor.fetchall()

    cursor.execute('SELECT art_No FROM Ingersoll WHERE product_No IS NULL AND product_Link IS NULL')
    art_numbers = cursor.fetchall()


    # Open a file to store URLs with no product number or link
    failed_urls_file = open('failed_urls.txt', 'w')

    # Loop through each article number
    for art_number_tuple in art_numbers:
        art_number = art_number_tuple[0]
        search_url = f"https://webshop.ingersoll-imc.de/search/catalogue/group/1?kw={art_number}"

        # Call the get_product_number function
        try:
            result_number, result_url = await asyncio.wait_for(get_product_number(search_url), timeout=8)
            if result_number and result_url:
                # Insert the product number and link into the Ingersoll table immediately
                cursor.execute('UPDATE Ingersoll SET product_No = ?, product_Link = ? WHERE art_No = ?',
                               (result_number, result_url, art_number))
                print(f"Updated art_No {art_number} with product_No {result_number} and product_Link {result_url}")
            else:
                print(f"No product found for art_No: {art_number}")
                # Write the failed URL to the failed_urls.txt file
                failed_urls_file.write(f"{search_url}\n")

        except asyncio.TimeoutError:
            print(f"Timeout exceeded for art_No: {art_number}")
            # Write the failed URL to the failed_urls.txt file
            failed_urls_file.write(f"{search_url}\n")

        # Commit changes after each update (live updates)
        conn.commit()

    # Close the database connection
    conn.close()

    # Close the failed URLs file
    failed_urls_file.close()

# Run the async main function
asyncio.run(main())
