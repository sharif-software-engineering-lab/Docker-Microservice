# Docker-Microservice
Deploy a Software with Microservice Architecture via Docker

<div dir="rtl">

## روال انجام آزمایش

ابتدا از نصب بودن داکر بر روی سیستم خود اطمینان حاصل می‌کنیم.

![1](Images/1.png)

حال با دستور زیر یک پروژه 
Web API
با استفاده از
.NET
ایجاد می‌کنیم.

```bash
mkdir CRUD-Service
dotnet new webapi -o Converter-Service 
```

کدهای مربوطه در این پروژه برای خواندن، نوشتن، بروزرسانی و حذف داده از دیتابیس 
PostgreSQL
نوشته شده‌اند. حال به سراغ 
containerize 
کردن پروژه می‌رویم. برای این کار داکرفایل زیر را می‌نویسیم. 

![2](Images/2.png)

در این فایل ابتدا با استفاده از ایمیجی که دارای 
DotNet SDK
است، پروژه را بیلد می‌کنیم سپس فایل‌های بیلد شده را به ایمیجی که دارای 
DotNet Runtime
است منتقل می‌کنیم. این کار برای سبک‌تر شدن ایمیج نهایی است. در ادامه باید داکر کامپوز مربوط به این سرویس و دیتابیس را بنویسیم. 

![3](Images/3.png)

همانطور که مشاهده می‌کنیم ابتدا ورژن کامپوز مشخص شده، سپس قسمت مربوط به دیتابیس را مشاهده می‌کنیم و پس از آن قسمت مربوط به ماکروسرویس اول. در آخر والیوم‌های مورد نیاز معرفی شده‌اند. با دستور زیر کانتینرها را ساخته و اجرا می‌کنیم. 

![4](Images/4.png)

حال مشاهده می‌کنیم در مرورگر به درستی درخواست ارسال می‌شود. 

![5](Images/5.png)

در تصویر زیر ایمیج‌ها و کانتینرهای ایجاد شده را مشاهده می‌کنیم. 

![6](Images/6.png)

برای تست صحت عملکرد میکروسرویس از curl استفاده می‌کنیم. 
ابتدا تمام داده را پاک می‌کنیم.

![7](Images/7.png)

![8](Images/8.png)

حال به اضافه کردن داده می‌پردازیم.

![9](Images/9.png)

مشاهده می‌کنیم که سرویس به درستی کار می‌کند.

اگر احیانا فشار روی سرویس زیاد شود، به راحتی می‌توانیم از قابلیت 
load balancing
موجود در 
Docker Compose
استفاده کنیم. برای این منظور هنگام اجرای سرویس از آپشن 
scale
استفاده می‌کنیم. 

البته قبل از این کار باید فایل 
compose.yml
را به گونه‌ای تغییر دهیم که از شبکه 
host
استفاده نکند. چون در این صورت پورت‌ها تداخل پیدا می‌کنند. پس یک شبکه جدید برای کانتینرها تعریف می‌کنیم.

![10](Images/10.png)

حال با دستور زیر سرویس را اجرا می‌کنیم.

![11](Images/11.png)

در خروجی لیست کانتینرها مشاهده میکنیم که دو کانتینر برای 
crud-service
وجود دارد. که یکی به پورت 
8084
و دیگری به پورت
8085
بایند شده است. حال مشتری‌ها را بین این دو پورت تقسیم می‌کنیم تا لود سیستم کم شود.

![12](Images/12.png)

همچنین می‌توانیم با استفاده از 
Docker Swarm
سرویس را روی چندین ماشین اجرا کنیم.

در آخر برای انجام عملیات 
load balancing 
از ایمیج 
nginx
استفاده می‌کنیم. فایل کانفیگ آن در تصویر زیر قابل مشاهده است. 

![13](Images/13.png)

در این فایل ابتدا تعداد تردهای پاسخگو تعیین شده‌اند، سپس گفته شده است در پاسخ به درخواست‌های 
http
درخواست‌ها به لیستی از سرورها که مشخص شده‌اند فرستاده شود. 

فایل 
comopse.yml
هم باید به صورتی تغییر کند که 
nginx 
به آن اضافه شود.

![14](Images/14.png)

در قسمت زیر اجرا شدن همه سرویس‌ها با هم را مشاهده می‌کنیم.

![15](Images/15.png)
![16](Images/16.png)

مشاهده می‌کنیم وقتی به 
nginx 
که در پورت
9000
مستقر است درخواست ارسال می‌کنیم به درستی به 
سرویسمان منتقل می‌شود.

![17](Images/17.png)

## پرسش‌ها


</div>
