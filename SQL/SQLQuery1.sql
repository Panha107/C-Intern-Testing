use ProductSales;
CREATE TABLE PRODUCTSALES(
	SALEID INT PRIMARY KEY,
	PRODUCTCODE NVARCHAR(20),
	PRODUCTNAME NVARCHAR(100),
	QUANTITY INT,
	UNITPRICE DECIMAL(18, 2),
	SALEDATE DATE
);

INSERT INTO PRODUCTSALES (SALEID, PRODUCTCODE, PRODUCTNAME, QUANTITY, UNITPRICE, SALEDATE)
VALUES
(1,  'P003', 'Ruler',    10, 0.50, '2025-06-20'),
(2,  'P001', 'Pen',       5, 1.50, '2025-06-25'),
(3,  'P002', 'Notebook',  3, 3.20, '2025-06-21'),
(4,  'P003', 'Eraser',   15, 0.80, '2025-06-22'),
(6,  'P001', 'Pen',       5, 1.50, '2025-06-25'),
(7,  'P001', 'Pen',      10, 1.50, '2025-06-20'),
(8,  'P001', 'Pen',       5, 1.50, '2025-06-25'),
(10, 'P001', 'Pen',      15, 1.50, '2025-06-20'),
(11, 'P003', 'Pen',      15, 1.50, '2025-06-20');


SELECT *FROM PRODUCTSALES

DELETE From PRODUCTSALES where SALEID=9
