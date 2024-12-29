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






from flask import Flask, request, jsonify, render_template
from flask_cors import CORS
from moveToolWebAPI import main  # Import the `main` class from your other Python program

app = Flask(__name__)
CORS(app)

@app.route('/')
def index():
    return render_template('main.html')  # Flask will look for 'main.html' in the 'templates' folder

@app.route('/place', methods=['POST'])

def place():
    data = request.json
    query = data.get('query')  # Get the query value
    tool_place = data.get('tool_place')  # Get the row[19] value

    print(f"Query: {query}, ToolPlace: {tool_place}")  # Print all values in the terminal

    # Call the `main` class's __init__ method, passing query and tool_place
    try:
        main(query, tool_place)  # Create an instance of `main` with query and toolPlace
        message = "Operation completed successfully."  # Success message
    except Exception as e:
        print(f"Error occurred: {e}")
        return jsonify({"status": "error", "message": str(e)}), 500

    return jsonify({"status": "success", "message": message, "query": query, "tool_place": tool_place})


if __name__ == '__main__':
    app.run(host='0.0.0.0', port=5000, ssl_context=('cert.pem', 'key.pem'), debug=True)
