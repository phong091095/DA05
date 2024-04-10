create database C#4_ASM;
use C#4_ASM;

create table Products(
	Pro_ID int Identity(1,1) primary key,
	Pro_Name nvarchar(255),
	Quantity int,
	Pro_Brand nvarchar(255) ,
	Pro_Type nvarchar(255) ,
	Ram nvarchar(255),
	CPU nvarchar(255),
	HDH nvarchar(255),
	Camera nvarchar(255),
	Color nvarchar(255),
	Pro_Price decimal(10,2),
	Pro_Img varbinary(Max)
);
alter table Products
alter column Pro_Type nvarchar(255);
create table Customer(
	CustID int identity(1,1) primary key,
	CustName nvarchar(255),
	CustPhone varchar(20),
	CustMail varchar(255),
	CustAd nvarchar(255),
	Login_ID int constraint FK_Login foreign key (Login_ID) references Login_Info(Login_ID)
);
create table Invoice(
	InvoiceID int Identity(1,1) primary key,
	CustID int constraint FK_ICustID foreign key (CustID) references Customer(CustID),
	InvoiceDate Date,
	TotalAmount decimal(10,2),
	InvoiceStatus nvarchar(255),
	CustName NVARCHAR(255),
    CustPhone NVARCHAR(20),
    CustAdd NVARCHAR(255)

);


Alter table InvoiceDetail
CREATE TABLE InvoiceDetail (
    InvoiceDetailID INT IDENTITY(1,1) PRIMARY KEY,
    InvoiceID INT CONSTRAINT FK_InID FOREIGN KEY (InvoiceID) REFERENCES Invoice(InvoiceID),
    ProductID INT CONSTRAINT FK_InProID FOREIGN KEY (ProductID) REFERENCES Products(Pro_ID),
    Quantity INT,
    Subtotal DECIMAL(10,2)
);


create table Login_Info(
	Login_ID int identity(1,1) primary key,
	UserName varchar(255) not null unique,
	Passw varchar(max) not null,
	Email nvarchar(255) unique,
	Login_Role int not null,
	Login_Status bit not null
);


insert into Products(
	Pro_Name ,
	Quantity ,
	Pro_Brand ,
	Pro_Type,
	Ram , 
	CPU ,
	HDH ,
	Camera ,
	Color ,
	Pro_Price,
	Pro_Img )
	values ('IPhone 15 ProMax 256GB',100,'IPhone',N'Điện thoại','8 GB','Apple A17 Pro','iOS 17','12.0 MP','Titan Xanh','1100',(SELECT BulkColumn FROM OPENROWSET(BULK N'E:\C#4\ASM\ASM\ASM\wwwroot\Img_ASM\IPhone-15PM-256G.webp', SINGLE_BLOB) AS Image)),
	('Oppo Reno11 256G',100,'OPPO',N'Điện thoại','8 GB','Dimensity 7050','Android ColorOS 14','64.0 MP','Xanh đen','1100',(SELECT BulkColumn FROM OPENROWSET(BULK N'E:\C#4\ASM\ASM\ASM\wwwroot\Img_ASM\Oppo-Reno11_256G.webp', SINGLE_BLOB) AS Image)),
	('SamSung Galaxy S24 Ultra 5G 256G',100,'SamSung',N'Điện thoại','12 GB','Snapdragon 8 Gen 3','Android 14','200.0 MP','Xám Titan','1100',(SELECT BulkColumn FROM OPENROWSET(BULK N'E:\C#4\ASM\ASM\ASM\wwwroot\Img_ASM\S24Utra-256G.webp', SINGLE_BLOB) AS Image)),
	('SamSung Galaxy S22 5G 128G',100,'SamSung',N'Điện thoại','8 GB','Snapdragon 8 Gen 1','Android 12','50.0 MP','Tím','1100',(SELECT BulkColumn FROM OPENROWSET(BULK N'E:\C#4\ASM\ASM\ASM\wwwroot\Img_ASM\S22-128G.webp', SINGLE_BLOB) AS Image)),
	('Xiaomi Redmi 13C 128GB',100,'Xiaomi',N'Điện thoại','6 GB','Helio G85','Android 12','50.0 MP','Xanh','1100',(SELECT BulkColumn FROM OPENROWSET(BULK N'E:\C#4\ASM\ASM\ASM\wwwroot\Img_ASM\Xiaomi-13C-128G.webp', SINGLE_BLOB) AS Image)),
	('Samsung Galaxy Z Flip4 5G 128GB',100,'SamSung',N'Điện thoại','8 GB','Snapdragon 8+ Gen 1','Android 12','12.0 MP ','Tím','1100',(SELECT BulkColumn FROM OPENROWSET(BULK N'E:\C#4\ASM\ASM\ASM\wwwroot\Img_ASM\ZFlip4-128G.webp', SINGLE_BLOB) AS Image)),
	('Samsung Galaxy Z Fold5 5G 256GB',100,'SamSung',N'Điện thoại','12 GB','Snapdragon 8 Gen 2','Android 13.0','50 MP ','Xanh','1100',(SELECT BulkColumn FROM OPENROWSET(BULK N'E:\C#4\ASM\ASM\ASM\wwwroot\Img_ASM\ZFlod5-256G.webp', SINGLE_BLOB) AS Image))


	insert into Products(
	Pro_Name ,
	Quantity ,
	Pro_Brand ,
	Pro_Type,
	Ram , 
	CPU ,
	HDH ,
	Camera ,
	Color ,
	Pro_Price,
	Pro_Img )
	values ('Xiaomi 14 256GB',100,'Xiaomi',N'Điện thoại','12 GB','Snapdragon 8 Gen 3','Android 14','50MP','Xanh','21000000',(SELECT BulkColumn FROM OPENROWSET(BULK N'E:\C#4\ASM\Webphone\Webphone\wwwroot\img\Xiaomi-14-256G.webp', SINGLE_BLOB) AS Image)),
	('Oppo A77s 128GB',100,'Oppo',N'Điện thoại','8 GB','Snapdragon 680 4G','Android 12','50 MP','Xanh','5190000',(SELECT BulkColumn FROM OPENROWSET(BULK N'E:\C#4\ASM\Webphone\Webphone\wwwroot\img\Oppo-A77s-128G.webp', SINGLE_BLOB) AS Image)),
	('Realme C51 256G',100,'Realme',N'Điện thoại','6 GB','Unisoc UNISOC T612','realme UI T Edition - Android 13','50 MP','Xanh','4090000',(SELECT BulkColumn FROM OPENROWSET(BULK N'E:\C#4\ASM\Webphone\Webphone\wwwroot\img\Realme-C51-256G.webp', SINGLE_BLOB) AS Image)),
	('ROG Phone 6 256G',100,'Asus ROG Phone',N'Điện thoại','12 GB','Qualcomm ® Snapdragon ® 8+ thế hệ 1','Android 12','50 MP',N'Trắng','14490000',(SELECT BulkColumn FROM OPENROWSET(BULK N'E:\C#4\ASM\Webphone\Webphone\wwwroot\img\Rogphone-6-256G.webp', SINGLE_BLOB) AS Image)),
	('ROG Phone 7 Ultimate',100,'Asus ROG Phone',N'Điện thoại','16 GB','Qualcomm® Snapdragon™ 8 Gen 2','Android 13','50 MP',N'Trắng','26190000',(SELECT BulkColumn FROM OPENROWSET(BULK N'E:\C#4\ASM\Webphone\Webphone\wwwroot\img\RogPhone7-512G.webp', SINGLE_BLOB) AS Image))