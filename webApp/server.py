# from flask import Flask, request, jsonify, render_template
# from flask_cors import CORS

# app = Flask(__name__)
# CORS(app)

# @app.route('/')
# def index():
#     return render_template('main.html')  # Flask will look for 'main.html' in the 'templates' folder

# @app.route('/place', methods=['POST'])
# def place():
#     data = request.json
#     query = data.get('query')  # Get the query value
#     tool_place = data.get('tool_place')  # Get the row[19] value
#     # entered_value = data.get('entered_value')  # Get the entered value from the input field

#     print(f"Query: {query}, ToolPlace: {tool_place}")  # Print all values in the terminal


#     return jsonify({"status": "success", "query": query, "tool_place": tool_place})


# if __name__ == '__main__':
#     app.run(host='0.0.0.0', port=5000, debug=True)


from flask import Flask, request, jsonify, render_template, send_from_directory, redirect, url_for
from flask_cors import CORS
import json
from moveToolWebAPI import main  # Import the `main` class from your other Python program

app = Flask(__name__)
CORS(app)

# Define the path to your images folder
IMAGE_FOLDER = 'D:/Projekt/mtm/data/toolCapturedImages'

# Path to the confidentialIP.json file
CONFIDENTIAL_IP_FILE = 'confidentialIP.json'

# Load allowed IPs and devices from confidentialIP.json
def load_allowed_ips():
    with open(CONFIDENTIAL_IP_FILE, 'r') as f:
        return json.load(f)

# Save the new allowed IP list with device names to confidentialIP.json
def save_allowed_ips(data):
    with open(CONFIDENTIAL_IP_FILE, 'w') as f:
        json.dump(data, f, indent=4)

# Check if the client IP is allowed
def is_allowed_ip():
    client_ip = request.remote_addr  # Get the client's IP address
    allowed_ips = load_allowed_ips()
    return any(entry['ip'] == client_ip for entry in allowed_ips)

@app.route('/')
def index():
    if is_allowed_ip():
        return render_template('main.html')  # Show the main page if IP is allowed
    else:
        return render_template('auth.html')  # Show login page if IP is not allowed

# Serve image files from the folder
@app.route('/images/<path:filename>')
def serve_image(filename):
    return send_from_directory(IMAGE_FOLDER, filename)

# Handle the place operation
@app.route('/place', methods=['POST'])
def place():
    data = request.json
    query = data.get('query')
    tool_place = data.get('tool_place')

    print(f"Query: {query}, ToolPlace: {tool_place}")

    try:
        message="now trying"
        main(query, tool_place)
        message = "Operation completed successfully."
    except Exception as e:
        print(f"Error occurred: {e}")
        return jsonify({"status": "error", "message": str(e)}), 500

    return jsonify({"status": "success", "message": message, "query": query, "tool_place": tool_place})

# Handle user login and IP storage with device name
@app.route('/login', methods=['POST'])
def login():
    username = request.form.get('username')
    password = request.form.get('password')
    device_name = request.form.get('device_name')  # Get the device name from the form
    client_ip = request.remote_addr  # Get the user's IP address

    # Dummy credentials check (replace with your actual logic)
    if username == 'admin' and password == 'password':  # Example credentials
        allowed_ips = load_allowed_ips()

        # Check if the IP is already in the list
        if not any(entry['ip'] == client_ip for entry in allowed_ips):
            # Add the client IP and device name to the list if it's not already there
            allowed_ips.append({'ip': client_ip, 'device_name': device_name})
            save_allowed_ips(allowed_ips)  # Save the updated list with IP and device names

        return redirect(url_for('index'))  # Redirect to the main page after login
    else:
        return render_template('auth.html', error="Invalid username or password")  # Reload the login page with an error

if __name__ == '__main__':
    app.run(host='0.0.0.0', port=5000, ssl_context=('cert.pem', 'key.pem'), debug=True)
