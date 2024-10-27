
Project setup
	SMTP
	https://support.google.com/mail/thread/182882752/application-specific-password-required?hl=en
	Google account > Bảo mật > Xác thực 2 lớp > App passwords
	Email template
	https://beefree.io/start-designing?template=reset-your-password&type=email&catalog=true&e=true



	create order sample
	{
    "payment_type_id": 2,
    "note": "Tintest 123",
    "required_note": "KHONGCHOXEMHANG",
    "from_name": "TinTest124",
    "from_phone": "0974255412",
    "from_address": "84A Trần Hữu Trang, Phường 10, Quận Phú Nhuận, Hồ Chí Minh, Vietnam",
    "from_ward_name": "Phường 14",
    "from_district_name": "Quận 10",
    "from_province_name": "HCM",
    "return_phone": "0332190444",
    "return_address": "39 NTT",
    "return_district_id": null,
    "return_ward_code": "",
    "client_order_code": "TUTUUA1",
    "to_name": "TinTest124",
    "to_phone": "0987654321",
    "to_address": "72 Thành Thái, Phường 14, Quận 10, Hồ Chí Minh, Vietnam",
    "to_ward_code": "20308",
    "to_district_id": 1444,
    "cod_amount": 10000,
    "content": "Theo New York Times",
    "weight": 500,
    "length": 10,
    "width": 10,
    "height": 10,
    "pick_station_id": 1444,
    "deliver_station_id": null,
    "insurance_value": 500000,
    "service_id": 0,
    "service_type_id": 2,
    "coupon": null,
    "pick_shift": [
        2
    ],
    "items": [
        {
            "name": "Áo Polo",
            "code": "Polo123",
            "quantity": 1,
            "price": 50000,
            "length": 10,
            "width": 10,
            "height": 10,
            "weight": 500,
            "category": {
                "level1": "Áo"
            }
        }
    ]
}



{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=202.92.7.204;Initial Catalog=paatktblhosting_db_ghst;User ID=paatktblhosting_user;Password=Tsw2@k621;MultipleActiveResultSets=True;TrustServerCertificate=True",
    "IdentityConnection": "Data Source=202.92.7.204;Initial Catalog=paatktblhosting_db_ghst;User ID=paatktblhosting_user;Password=Tsw2@k621;MultipleActiveResultSets=True;TrustServerCertificate=True",
    "FileManagerConnection": "Data Source=202.92.7.204;Initial Catalog=paatktblhosting_db_ghst;User ID=paatktblhosting_user;Password=Tsw2@k621;MultipleActiveResultSets=True;TrustServerCertificate=True"
  },
  "JwtSettings": {
    "Key": "C1CF4B7DC4C4175B6618DE4F55CA4AAA",
    "Issuer": "GHST_Identity",
    "Audience": "GHST_IdentityUser",
    "DurationInMinutes": 1600
  },
  "SmtpSettings": {
    "ClientHost": "http://localhost:5555",
    "Server": "smtp.gmail.com",
    "Port": 587,
    "User": "mjsshunnjer@gmail.com",
    "Password": "aszo nvjz ykvl ylst"
  },
  "Env": "DEV",
  "EnableSwagger":  true
}