import re
from bs4 import BeautifulSoup
from flask import Flask

SMPL_103 = 3094687      # Super Monster Milk
SMPL_104 = 3063039      # Super Monster Milk
SMPL_XML = 4374         # Super Monster Milk
SMPL_XML = SMPL_XML + 1 # Offset Alignment

DIFF_103 = SMPL_103 - SMPL_XML
DIFF_104 = SMPL_104 - SMPL_XML

app = Flask(__name__)

@app.route("/")
def main():
    with open('shantae.ct', 'r') as f:
        file = f.read() 

    soup = BeautifulSoup(file, 'xml')

    root = soup \
            .find('CheatTable') \
            .find('CheatEntries') \
            .find('CheatEntry') \
            .find('CheatEntries') \
            .find_all('CheatEntry')

    items = []
    block = {
        'xml': [],
        '103': [],
        '104': [],
    }

    for section in root:
        entries = section.find('CheatEntries')
        
        if entries is None:
            continue

        for item in entries.find_all('CheatEntry'):
            offsets = item.find('Offsets')
            
            if offsets is None:
                continue
            
            offset = offsets.find('Offset')
            
            if offset is None:
                continue

            offset_xml = int(offset.get_text(), 16)
            offset_103 = offset_xml + DIFF_103
            offset_104 = offset_xml + DIFF_104

            block['xml'].append(offset_xml)
            block['103'].append(offset_103)
            block['104'].append(offset_104)

            description = re.sub(' +', ' ', 
                item.find('Description') \
                    .get_text() \
                    .replace('\n', ' ')\
                    .replace('\r', '')
                    .replace('"', ''))

            # if ("#" in description) or ("+" in description) or ("No description" in description):
            #     continue

            type = item.find('VariableType')

            if type is None:
                continue

            type = type.get_text()

            start = item.find('BitStart')
            length = item.find('BitLength')

            if start is not None:
                start = int(start.get_text())
            else:
                start = 0

            if length is not None:
                length = int(length.get_text())
            else:
                length = 0

            items.append({
                'item': description,
                'type': type,
                'start': start,
                'length': length,
                'offsets': {
                    'xml': offset_xml,
                    '103': offset_103,
                    '104': offset_104,
                },
            })

    min_xml = min(block['xml'])
    max_xml = max(block['xml'])
    dif_xml = max_xml - min_xml

    min_103 = min(block['103'])
    max_103 = max(block['103'])
    dif_103 = max_103 - min_103

    min_104 = min(block['104'])
    max_104 = max(block['104'])
    dif_104 = max_104 - min_104

    return {
        'block': {
            'xml': {
                'start': min_xml,
                'end': max_xml,
                'length': dif_xml
            },
            '103': {
                'start': min_103,
                'end': max_103,
                'length': dif_104
            },
            '104': {
                'start': min_104,
                'end': max_104,
                'length': dif_104
            }
        },
        'items': items,
    }