import json

# Load the JSON data from the config file
with open('../config/config.json', 'r') as file:  # Replace 'config.json' with the actual path to your JSON file
    config_data = json.load(file)

# Access the paths
imgpaths = config_data['paths']['imgpaths']
compimgs = config_data['paths']['compimgs']
toolstl = config_data['paths']['toolstl']
mastercsv = config_data['paths']['mastercsv']
TNC640_Daten=config_data['paths']['TNC640_Daten']
paths=config_data["paths"]
toolsremark=config_data["paths"]["toolsremark"]
# Access the machines list
places= config_data["places"]
winconfig= config_data['winconfig']
settings=config_data["settings"]


