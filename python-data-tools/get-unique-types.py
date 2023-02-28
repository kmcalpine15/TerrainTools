
#!/usr/bin/env python3

import pandas as pd
import glob
import os

with open('/Users/krismcalpine/Downloads/opname_csv_gb/Doc/OS_Open_Names_Header.csv') as fle:
    columns = fle.read().split(',')

allFiles = glob.glob('/Users/krismcalpine/Downloads/opname_csv_gb/Data/*.csv')

li=[]
for f in allFiles:
    df = pd.read_csv(f, names=columns)
    li.append(df)

allData = pd.concat(li, axis=0, ignore_index=True)
