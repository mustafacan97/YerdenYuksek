# Personel eCommerce Project

![alt text](https://api.mustafacan.co/eCommerceProjectSmall.jpg "Prject Logo")

Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum.

## Project Structure
1. Libraries
   - eCommerce.Application
   - eCommerce.Core
2. Presentation
   - eCommerce.Web
   - eCommerce.Web.Contract
   - eCommerce.Web.Framework

## Techologies 
1. .NET 6.0
2. Repository Pattern
3. Mediator Pattern (CQRS)
4. Entity Framework

## Tables And RelationShips
1. Customer
2. Address
3. Localization
4. Setting
5. CustomerRole
6. Permission

## Common Information

1. Create Migration
* dotnet ef migrations add InitialMigration --output-dir Migrations/{date} --startup-project "../YerdenYuksek.Web"

## Topics
#### Security
1. ***Role*** tabanlı bir Authorization yapısı kullanılmıştır.
2. Authentication ve Authorization işlemleri için ***JWT*** teknolojisi kullanılmıştır.
3. DB tarafında detaylı bilgi için ***Permission*** and ***PermissionCustomerRoleMapping*** tabloları incelenmelidir.
