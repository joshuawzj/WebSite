if exists (select * from sysobjects where id = OBJECT_ID('[Whir_Shop_AttrPro]') and OBJECTPROPERTY(id, 'IsUserTable') = 1) 
DROP TABLE [Whir_Shop_AttrPro]

CREATE TABLE [Whir_Shop_AttrPro] (
[AttrProID] [int]  IDENTITY (1, 1)  NOT NULL,
[ProID] [int]  NULL,
[AttrValueIDs] [nvarchar]  (512) NULL,
[AttrValueNames] [nvarchar]  (512) NULL,
[CostAmount] [decimal]  (20,2) NULL,
[ProImg] [nvarchar]  (512) NULL,
[Images] [nvarchar]  (MAX) NULL,
[IsUseMainImage] [bit]  NULL,
[IsDel] [bit]  NULL,
[State] [int]  NULL,
[Sort] [bigint]  NULL,
[CreateDate] [datetime]  NULL,
[CreateUser] [nvarchar]  (256) NULL,
[UpdateDate] [datetime]  NULL,
[UpdateUser] [nvarchar]  (256) NULL)

ALTER TABLE [Whir_Shop_AttrPro] WITH NOCHECK ADD  CONSTRAINT [PK_Whir_Shop_AttrPro] PRIMARY KEY  NONCLUSTERED ( [AttrProID] )

if exists (select * from sysobjects where id = OBJECT_ID('[Whir_Shop_Attr]') and OBJECTPROPERTY(id, 'IsUserTable') = 1) 
DROP TABLE [Whir_Shop_Attr]

CREATE TABLE [Whir_Shop_Attr] (
[AttrID] [int]  IDENTITY (1, 1)  NOT NULL,
[SearchName] [nvarchar]  (256) NULL,
[IsShowImage] [bit]  NULL,
[IsDel] [bit]  NULL,
[State] [int]  NULL,
[Sort] [bigint]  NULL,
[CreateDate] [datetime]  NULL,
[CreateUser] [nvarchar]  (256) NULL,
[UpdateDate] [datetime]  NULL,
[UpdateUser] [nvarchar]  (256) NULL)

ALTER TABLE [Whir_Shop_Attr] WITH NOCHECK ADD  CONSTRAINT [PK_Whir_Shop_Attr] PRIMARY KEY  NONCLUSTERED ( [AttrID] )

if exists (select * from sysobjects where id = OBJECT_ID('[Whir_Shop_AttrValue]') and OBJECTPROPERTY(id, 'IsUserTable') = 1) 
DROP TABLE [Whir_Shop_AttrValue]

CREATE TABLE [Whir_Shop_AttrValue] (
[AttrValueID] [int]  IDENTITY (1, 1)  NOT NULL,
[AttrValueName] [nvarchar]  (256) NULL,
[AttrID] [int]  NULL,
[ShowImage] [nvarchar]  (256) NULL,
[IsDel] [bit]  NULL,
[State] [int]  NULL,
[Sort] [bigint]  NULL,
[CreateDate] [datetime]  NULL,
[CreateUser] [nvarchar]  (256) NULL,
[UpdateDate] [datetime]  NULL,
[UpdateUser] [nvarchar]  (256) NULL)

ALTER TABLE [Whir_Shop_AttrValue] WITH NOCHECK ADD  CONSTRAINT [PK_Whir_Shop_AttrValue] PRIMARY KEY  NONCLUSTERED ( [AttrValueID] )

if exists (select * from sysobjects where id = OBJECT_ID('[Whir_Shop_Cart]') and OBJECTPROPERTY(id, 'IsUserTable') = 1) 
DROP TABLE [Whir_Shop_Cart]

CREATE TABLE [Whir_Shop_Cart] (
[CartID] [int]  IDENTITY (1, 1)  NOT NULL,
[UniqueID] [nvarchar]  (128) NULL,
[ProID] [int]  NULL,
[AttrProID] [int]  NULL,
[Qutity] [int]  NULL,
[IsDel] [bit]  NULL,
[State] [int]  NULL,
[Sort] [bigint]  NULL,
[CreateDate] [datetime]  NULL,
[CreateUser] [nvarchar]  (256) NULL,
[UpdateDate] [datetime]  NULL,
[UpdateUser] [nvarchar]  (256) NULL)

ALTER TABLE [Whir_Shop_Cart] WITH NOCHECK ADD  CONSTRAINT [PK_Whir_Shop_Cart] PRIMARY KEY  NONCLUSTERED ( [CartID] )

if exists (select * from sysobjects where id = OBJECT_ID('[Whir_Shop_Category]') and OBJECTPROPERTY(id, 'IsUserTable') = 1) 
DROP TABLE [Whir_Shop_Category]

CREATE TABLE [Whir_Shop_Category] (
[CategoryID] [int]  IDENTITY (1, 1)  NOT NULL,
[ParentID] [int]  NULL,
[ParentPath] [nvarchar]  (512) NULL,
[CategoryName] [nvarchar]  (256) NULL,
[MetaTitle] [nvarchar]  (128) NULL,
[MetaKeyword] [nvarchar]  (256) NULL,
[MetaDescription] [nvarchar]  (1024) NULL,
[IsDel] [bit]  NULL,
[State] [int]  NULL,
[Sort] [bigint]  NULL,
[CreateDate] [datetime]  NULL,
[CreateUser] [nvarchar]  (256) NULL,
[UpdateDate] [datetime]  NULL,
[UpdateUser] [nvarchar]  (256) NULL)

ALTER TABLE [Whir_Shop_Category] WITH NOCHECK ADD  CONSTRAINT [PK_Whir_Shop_Category] PRIMARY KEY  NONCLUSTERED ( [CategoryID] )

if exists (select * from sysobjects where id = OBJECT_ID('[Whir_Shop_Consult]') and OBJECTPROPERTY(id, 'IsUserTable') = 1) 
DROP TABLE [Whir_Shop_Consult]

CREATE TABLE [Whir_Shop_Consult] (
[ConsultID] [int]  IDENTITY (1, 1)  NOT NULL,
[ProID] [int]  NULL,
[MemberID] [int]  NULL,
[ConsultSaleAmount] [decimal]  (10,4) NULL,
[Consult] [nvarchar]  (MAX) NULL,
[Reply] [nvarchar]  (MAX) NULL,
[ReplyDate] [datetime]  NULL,
[IsDel] [bit]  NULL,
[State] [int]  NULL,
[Sort] [bigint]  NULL,
[CreateDate] [datetime]  NULL,
[CreateUser] [nvarchar]  (256) NULL,
[UpdateDate] [datetime]  NULL,
[UpdateUser] [nvarchar]  (256) NULL,
[ReplyUser] [nvarchar]  (64) NULL)

ALTER TABLE [Whir_Shop_Consult] WITH NOCHECK ADD  CONSTRAINT [PK_Whir_Shop_Consult] PRIMARY KEY  NONCLUSTERED ( [ConsultID] )

if exists (select * from sysobjects where id = OBJECT_ID('[Whir_Shop_Courier]') and OBJECTPROPERTY(id, 'IsUserTable') = 1) 
DROP TABLE [Whir_Shop_Courier]

CREATE TABLE [Whir_Shop_Courier] (
[CourierID] [int]  IDENTITY (1, 1)  NOT NULL,
[CourierName] [nvarchar]  (256) NULL,
[Interface] [nvarchar]  (256) NULL,
[IsDel] [bit]  NULL,
[State] [int]  NULL,
[Sort] [bigint]  NULL,
[CreateDate] [datetime]  NULL,
[CreateUser] [nvarchar]  (256) NULL,
[UpdateDate] [datetime]  NULL,
[UpdateUser] [nvarchar]  (256) NULL,
[Com] [varchar]  (120) NULL)

ALTER TABLE [Whir_Shop_Courier] WITH NOCHECK ADD  CONSTRAINT [PK_Whir_Shop_Courier] PRIMARY KEY  NONCLUSTERED ( [CourierID] )

if exists (select * from sysobjects where id = OBJECT_ID('[Whir_Shop_Field]') and OBJECTPROPERTY(id, 'IsUserTable') = 1) 
DROP TABLE [Whir_Shop_Field]

CREATE TABLE [Whir_Shop_Field] (
[FieldID] [int]  IDENTITY (1, 1)  NOT NULL,
[FieldName] [nvarchar]  (32) NULL,
[FieldAlias] [nvarchar]  (64) NULL,
[FieldType] [nvarchar]  (32) NULL,
[IsAllowNull] [bit]  NULL,
[ShowType] [int]  NULL,
[ValidateExpression] [nvarchar]  (512) NULL,
[ValidateType] [int]  NULL,
[TipText] [nvarchar]  (512) NULL,
[ValidateText] [nvarchar]  (512) NULL,
[BindType] [int]  NULL,
[BindText] [nvarchar]  (1024) NULL,
[BindSql] [nvarchar]  (512) NULL,
[BindTable] [nvarchar]  (64) NULL,
[BindKeyID] [int]  NULL,
[BindValueField] [nvarchar]  (64) NULL,
[BindTextField] [nvarchar]  (64) NULL,
[RepeatColumn] [int]  NULL,
[DefaultValue] [nvarchar]  (256) NULL,
[IsUsing] [bit]  NULL,
[IsDel] [bit]  NULL,
[State] [int]  NULL,
[Sort] [bigint]  NULL,
[CreateDate] [datetime]  NULL,
[CreateUser] [nvarchar]  (256) NULL,
[UpdateDate] [datetime]  NULL,
[UpdateUser] [nvarchar]  (256) NULL)

ALTER TABLE [Whir_Shop_Field] WITH NOCHECK ADD  CONSTRAINT [PK_Whir_Shop_Field] PRIMARY KEY  NONCLUSTERED ( [FieldID] )

if exists (select * from sysobjects where id = OBJECT_ID('[Whir_Shop_MemberInfo]') and OBJECTPROPERTY(id, 'IsUserTable') = 1) 
DROP TABLE [Whir_Shop_MemberInfo]

CREATE TABLE [Whir_Shop_MemberInfo] (
[MemberID] [int]  IDENTITY (1, 1)  NOT NULL,
[LoginName] [nvarchar]  (64) NULL,
[Password] [nvarchar]  (512) NULL,
[Email] [nvarchar]  (128) NULL,
[Sex] [nvarchar]  (8) NULL,
[Birthday] [datetime]  NULL,
[NickName] [nvarchar]  (128) NULL,
[Consignee] [nvarchar]  (64) NULL,
[Address] [nvarchar]  (512) NULL,
[Tel] [nvarchar]  (32) NULL,
[Mobile] [nvarchar]  (32) NULL,
[Postcode] [nvarchar]  (16) NULL,
[ConEmail] [nvarchar]  (128) NULL,
[IsDel] [bit]  NULL,
[State] [int]  NULL,
[Sort] [bigint]  NULL,
[CreateDate] [datetime]  NULL,
[CreateUser] [nvarchar]  (256) NULL,
[UpdateDate] [datetime]  NULL,
[UpdateUser] [nvarchar]  (256) NULL)

ALTER TABLE [Whir_Shop_MemberInfo] WITH NOCHECK ADD  CONSTRAINT [PK_Whir_Shop_MemberInfo] PRIMARY KEY  NONCLUSTERED ( [MemberID] )

if exists (select * from sysobjects where id = OBJECT_ID('[Whir_Shop_OrderInfo]') and OBJECTPROPERTY(id, 'IsUserTable') = 1) 
DROP TABLE [Whir_Shop_OrderInfo]

CREATE TABLE [Whir_Shop_OrderInfo] (
[OrderID] [int]  IDENTITY (1, 1)  NOT NULL,
[OrderNo] [nvarchar]  (128) NULL,
[MemberID] [int]  NULL,
[CourierID] [int]  NULL,
[IsPaid] [bit]  NULL,
[Status] [int]  NULL,
[IsCancel] [bit]  NULL,
[ShipOrderNumber] [nvarchar]  (64) NULL,
[ProductAmount] [decimal]  (20,2) NULL,
[DiscountAmount] [decimal]  (20,2) NULL,
[PayAmount] [decimal]  (20,2) NULL,
[PaymentID] [int]  NULL,
[FinishDate] [datetime]  NULL,
[TakeName] [nvarchar]  (64) NULL,
[TakeMobile] [nvarchar]  (32) NULL,
[TakeTel] [nvarchar]  (64) NULL,
[TakePostcode] [nvarchar]  (16) NULL,
[TakeRegion] [int]  NULL,
[TakeAddress] [nvarchar]  (256) NULL,
[TakeEmail] [nvarchar]  (128) NULL,
[TakeInvoice] [nvarchar]  (2048) NULL,
[IsDel] [bit]  NULL,
[State] [int]  NULL,
[Sort] [bigint]  NULL,
[CreateDate] [datetime]  NULL,
[CreateUser] [nvarchar]  (256) NULL,
[UpdateDate] [datetime]  NULL,
[UpdateUser] [nvarchar]  (256) NULL)

ALTER TABLE [Whir_Shop_OrderInfo] WITH NOCHECK ADD  CONSTRAINT [PK_Whir_Shop_OrderInfo] PRIMARY KEY  NONCLUSTERED ( [OrderID] )

if exists (select * from sysobjects where id = OBJECT_ID('[Whir_Shop_OrderProduct]') and OBJECTPROPERTY(id, 'IsUserTable') = 1) 
DROP TABLE [Whir_Shop_OrderProduct]

CREATE TABLE [Whir_Shop_OrderProduct] (
[OrderProID] [int]  IDENTITY (1, 1)  NOT NULL,
[OrderID] [int]  NULL,
[ProID] [int]  NULL,
[AttrProID] [int]  NULL,
[ProNO] [nvarchar]  (32) NULL,
[ProName] [nvarchar]  (512) NULL,
[SaleAmount] [decimal]  (20,2) NULL,
[Count] [int]  NULL,
[IsDel] [bit]  NULL,
[State] [int]  NULL,
[Sort] [bigint]  NULL,
[CreateDate] [datetime]  NULL,
[CreateUser] [nvarchar]  (256) NULL,
[UpdateDate] [datetime]  NULL,
[UpdateUser] [nvarchar]  (256) NULL)

ALTER TABLE [Whir_Shop_OrderProduct] WITH NOCHECK ADD  CONSTRAINT [PK_Whir_Shop_OrderProduct] PRIMARY KEY  NONCLUSTERED ( [OrderProID] )

if exists (select * from sysobjects where id = OBJECT_ID('[Whir_Shop_ProInfo]') and OBJECTPROPERTY(id, 'IsUserTable') = 1) 
DROP TABLE [Whir_Shop_ProInfo]

CREATE TABLE [Whir_Shop_ProInfo] (
[ProID] [int]  IDENTITY (1, 1)  NOT NULL,
[ProNO] [nvarchar]  (32) NULL,
[ProName] [nvarchar]  (512) NULL,
[CategoryID] [int]  NULL,
[IsAllowBuy] [bit]  NULL,
[ProDesc] [nvarchar]  (4000) NULL,
[CostAmount] [decimal]  (20,2) NULL,
[ProImg] [nvarchar]  (512) NULL,
[Images] [nvarchar]  (4000) NULL,
[SearchValueIDs] [nvarchar]  (4000) NULL,
[MetaTitle] [nvarchar]  (128) NULL,
[MetaKeyword] [nvarchar]  (256) NULL,
[MetaDescription] [nvarchar]  (1024) NULL,
[IsDel] [bit]  NULL,
[State] [int]  NULL,
[Sort] [bigint]  NULL,
[CreateDate] [datetime]  NULL,
[CreateUser] [nvarchar]  (256) NULL,
[UpdateDate] [datetime]  NULL,
[UpdateUser] [nvarchar]  (256) NULL,
[shijian] [datetime]  NULL,
[AttrIDs] [nvarchar]  (512) NULL)

ALTER TABLE [Whir_Shop_ProInfo] WITH NOCHECK ADD  CONSTRAINT [PK_Whir_Shop_ProInfo] PRIMARY KEY  NONCLUSTERED ( [ProID] )

if exists (select * from sysobjects where id = OBJECT_ID('[Whir_Shop_Search]') and OBJECTPROPERTY(id, 'IsUserTable') = 1) 
DROP TABLE [Whir_Shop_Search]

CREATE TABLE [Whir_Shop_Search] (
[SearchID] [int]  IDENTITY (1, 1)  NOT NULL,
[SearchName] [nvarchar]  (256) NULL,
[IsDel] [bit]  NULL,
[State] [int]  NULL,
[Sort] [bigint]  NULL,
[CreateDate] [datetime]  NULL,
[CreateUser] [nvarchar]  (256) NULL,
[UpdateDate] [datetime]  NULL,
[UpdateUser] [nvarchar]  (256) NULL)

ALTER TABLE [Whir_Shop_Search] WITH NOCHECK ADD  CONSTRAINT [PK_Whir_Shop_Search] PRIMARY KEY  NONCLUSTERED ( [SearchID] )

if exists (select * from sysobjects where id = OBJECT_ID('[Whir_Shop_SearchValue]') and OBJECTPROPERTY(id, 'IsUserTable') = 1) 
DROP TABLE [Whir_Shop_SearchValue]

CREATE TABLE [Whir_Shop_SearchValue] (
[SearchValueID] [int]  IDENTITY (1, 1)  NOT NULL,
[SearchID] [int]  NULL,
[SearchValueName] [nvarchar]  (256) NULL,
[IsDel] [bit]  NULL,
[State] [int]  NULL,
[Sort] [bigint]  NULL,
[CreateDate] [datetime]  NULL,
[CreateUser] [nvarchar]  (256) NULL,
[UpdateDate] [datetime]  NULL,
[UpdateUser] [nvarchar]  (256) NULL)

ALTER TABLE [Whir_Shop_SearchValue] WITH NOCHECK ADD  CONSTRAINT [PK_Whir_Shop_SearchValue] PRIMARY KEY  NONCLUSTERED ( [SearchValueID] )

