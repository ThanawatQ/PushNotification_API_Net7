>## Create table
>
```
CREATE TABLE `pushnotification` (
  `SubId` char(36) NOT NULL,
  `EndPoint` varchar(500) DEFAULT NULL,
  `P256DH` varchar(200) DEFAULT NULL,
  `Auth` varchar(100) DEFAULT NULL,
  `UserId` varchar(45) DEFAULT NULL,
  `Group` varchar(45) DEFAULT NULL,
  `IsDelete` tinyint DEFAULT NULL,
  PRIMARY KEY (`SubId`),
  UNIQUE KEY `EndPoint_UNIQUE` (`EndPoint`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci
```

>## JSOn  [HttpPost("notification")]

- ส่งแบบ หลาย Users
```
{
  "content": {
    "title": "หัวข้อ",
    "body": "เนื้อหา",
    "icon": "https://cdn-icons-png.flaticon.com/512/3135/3135715.png",
    "Type": "openlink",
    "url": [
        "https://www.google.com", 
         "https://www.google.com" 
    ],
    "actions": [
            {
              "action": "yes",
              "type": "button",
              "title": "👍 Yes"
            },
            {
              "action": "yes",
              "type": "button",
              "title": "👎 No (explain why)"
            }
        ]
   },
  "userId": [
    "Test UserId","NewUserId"
  ],
  "groups": [

  ],
  "sentAll": false
}
```
- ส่งแบบ กลุ่ม (ได้หลายกลุ่ม)
```
{
  "content": {
    "title": "หัวข้อ",
    "body": "เนื้อหา",
    "icon": "https://cdn-icons-png.flaticon.com/512/3135/3135715.png",
    "Type": "openlink",
    "url": [
        "https://www.google.com", 
         "https://www.google.com" 
    ],
    "actions": [
            {
              "action": "yes",
              "type": "button",
              "title": "👍 Yes"
            },
            {
              "action": "yes",
              "type": "button",
              "title": "👎 No (explain why)"
            }
        ]
   },
  "userId": [

  ],
  "groups": [
  "AMR","THAI"
  ],
  "sentAll": false
}
```

- ส่งแบบ ทั้งหมด
```
{
  "content": {
    "title": "หัวข้อ",
    "body": "เนื้อหา",
    "icon": "https://cdn-icons-png.flaticon.com/512/3135/3135715.png",
    "Type": "openlink",
    "url": [
        "https://www.google.com", 
         "https://www.google.com" 
    ],
    "actions": [
            {
              "action": "yes",
              "type": "button",
              "title": "👍 Yes"
            },
            {
              "action": "yes",
              "type": "button",
              "title": "👎 No (explain why)"
            }
        ]
   },
  "userId": [

  ],
  "groups": [
  
  ],
  "sentAll": true
}
```  
