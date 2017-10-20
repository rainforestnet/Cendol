# Cendol
Android Data Collector. 

This is a prototype that demonstrates how an android app captures user inputs (by keyboard or barcode scanning) and store them into a flat file on the device. 

A background service is invoked by a timer to read the new data in the flat file and post to a backend Web API.

Such model is particularly useful for rapid data input as and network connectivity is not guaranteed in certain working area such as manufacturing plants, rural site, warehouses and etc.

