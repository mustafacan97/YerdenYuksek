# Personel eCommerce Project

![alt text](https://api.mustafacan.co/eCommerceProjectSmall.jpg "Prject Logo")

Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum.

## Project Structure
1. Libraries
   - eCommerce.Application
   - eCommerce.Core
2. Presentation
   - eCommerce.Web
   - eCommerce.Web.Framework

## Techologies 
1. .NET 6.0
2. Repository Pattern
3. Mediator Pattern (CQRS)
4. Entity Framework

## Tables And RelationShips
#### Main Tables
1. ActivityLog
2. ActivityLogType
3. Address
4. Category
5. Country
6. City
7. Currency
8. Customer
9. CustomerSecurity
10. EmailAccount
11. Language
12. LocaleStringResource
13. LocalizedProperty
14. Log
15. Manufacturer
16. EmailTemplate
17. Order
18. OrderItem
19. OrderNote
20. Permission
21. Picture
22. Product
23. ProductAttribute
24. ProductAttributeValue
25. ProductAttributeValuePicture
26. QueuedEmail
27. ReturnRequest
28. ScheduleTask
29. Setting
30. Shipment
31. ShippingDeliveryDate
32. ShipmentItem
33. ShoppingCartItem
34. TaxCategory
35. Role
#### Mapping Tables
1. CustomerRoleMapping
2. CustomerAddressMapping
3. PermissionRoleMapping
4. ProductCategoryMapping
5. ProductManufacutrerMapping
6. ProductPictureMapping
7. ProductProductAttributeMapping

## Common Information

1. Create Migration
* dotnet ef migrations add InitialMigration --output-dir Migrations/{date} --startup-project "../YerdenYuksek.Web"

## Topics
#### Security
1. ***Role*** tabanlı bir Authorization yapısı kullanılmıştır.
2. Authentication ve Authorization işlemleri için ***JWT*** teknolojisi kullanılmıştır.
3. DB tarafında detaylı bilgi için ***Permission*** and ***PermissionCustomerRoleMapping*** tabloları incelenmelidir.
