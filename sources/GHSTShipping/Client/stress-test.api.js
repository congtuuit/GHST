const axios = require('axios');
const https = require('https');

// Create an HTTPS agent that ignores self-signed certificates
const agent = new https.Agent({
  rejectUnauthorized: false,
});

// Function to send the request
async function sendRequest() {
  let data = JSON.stringify({
    from_name: 'Văn Tú',
    from_phone: '0974255412',
    from_address: '60 Trường Sơn',
    from_ward_name: 'Phường 10',
    from_district_name: 'Quận Tân Bình',
    from_province_name: 'Hồ Chí Minh',
    pick_shift: [5],
    to_phone: '0968890960',
    to_name: 'Tú Tú',
    wardName: 'Phường Phước Bình',
    districtName: 'Thành Phố Thủ Đức',
    provinceName: 'Hồ Chí Minh',
    to_address: '72 Thành Thái',
    to_district_id: '3695',
    to_ward_code: '90762',
    items: [
      { name: 'Áo Thun', weight: 100, quantity: 2, code: '0010' },
      { name: 'Áo Polo', weight: 150, quantity: 2, code: '0020' },
    ],
    weight: 250,
    length: 10,
    cod_amount: 20000,
    insurance_value: 15000,
    required_note: 'CHOTHUHANG',
    note: 'Ghi Chú',
    payment_type_id: 2,
    cod_failed_amount: 11000,
    service_type_id: 2,
    service_id: 0,
  });

  let config = {
    method: 'post',
    maxBodyLength: Infinity,
    url: 'https://localhost:5001/api/v1/orders/ghn/create',
    headers: {
      accept: 'application/json, text/plain, */*',
      authorization:
        'Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJEaXNwbGF5TmFtZSI6IlRVIFZBTiIsIlR5cGUiOiJTSE9QIiwiaHR0cDovL3NjaGVtYXMueG1sc29hcC5vcmcvd3MvMjAwNS8wNS9pZGVudGl0eS9jbGFpbXMvbmFtZWlkZW50aWZpZXIiOiI4MTNjYzc3Ni03ZWM0LTQwZGYtNmMwNC0wOGRjZGY4ZjU0NGEiLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoiY29uZ3R1dWl0QGdtYWlsLmNvbSIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL2VtYWlsYWRkcmVzcyI6ImNvbmd0dXVpdEBnbWFpbC5jb20iLCJBc3BOZXQuSWRlbnRpdHkuU2VjdXJpdHlTdGFtcCI6IjRGUkZLWTMyVkRWSE5RQjRFUFJZU0JFSFRJWklDRFlHIiwiZXhwIjoxNzI4OTA1OTAyLCJpc3MiOiJHSFNUX0lkZW50aXR5IiwiYXVkIjoiR0hTVF9JZGVudGl0eVVzZXIifQ.B_CU7kR59jQJVdDSoVpqBkX6TfSxj1zlGJ-iakWxb2U',
      'content-type': 'application/json',
    },
    httpsAgent: agent,
    data: data,
  };

  try {
    const response = await axios.request(config);
    //console.log(JSON.stringify(response.data.success));
  } catch (error) {
   // console.log(error.message);
  }
}

async function spamAddresses() {
  let data = JSON.stringify({
    name: 'Home',
    address: '84 Đường Hùng Vương, Phường 9 (Quận 5), Quận 5, Hồ Chí Minh',
    addressDetail:
      '84 Đường Hùng Vương, Phường 9 (Quận 5), Quận 5, Hồ Chí Minh84 Đường Hùng Vương, Phường 9 (Quận 5), Q',
    note: '84 Đường Hùng Vương, Phường 9 (Quận 5), Quận 5, Hồ Chí Minh84 Đường Hùng Vương, Phường 9 (Quận 5), Q',
    accountId: '4607e1fe-bb60-43be-9156-08dce901c289',
    customerAddressTypeId: 0,
    lat: 10.7599879,
    lng: 106.6727615,
    numberPhone: '099110111',
    storeId: '824280ce-43e9-4235-2570-08dce101b936',
  });

  let config = {
    method: 'post',
    maxBodyLength: Infinity,
    url: 'https://api.fnb.pos.gosell.vn/api/v3.6/account/create-account-address',
    headers: {
      Accept: 'application/json',
      'Accept-Language': 'en-US,en;q=0.9',
      'Access-Control-Allow-Origin': '*',
      Authorization:
        'Bearer eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTI1NiIsInR5cCI6IkpXVCJ9.eyJJRCI6IjM5MzY4ZDgyLTQzOWMtNDcyMC0wYzMzLTA4ZGNlOTAxYzI4YiIsIkFDQ09VTlRfSUQiOiI0NjA3ZTFmZS1iYjYwLTQzYmUtOTE1Ni0wOGRjZTkwMWMyODkiLCJDUkVBVEVfREFURSI6IjEwLzEwLzIwMjQgODowMTozNCBBTSIsIkZVTExfTkFNRSI6IkxvbmcgQnVpIiwiQUNDT1VOVF9UWVBFIjoiMCIsIlNUT1JFX0lEIjoiODI0MjgwY2UtNDNlOS00MjM1LTI1NzAtMDhkY2UxMDFiOTM2IiwiRU1BSUwiOiJsb25nLmJ1aUB5b3BtYWlsLmNvbSIsIlBIT05FX05VTUJFUiI6Ijk5MTEwMTExIiwiQ09VTlRSWV9JRCI6ImVmMzc4M2MxLThmZjItNDQ2Yi05ZTYyLTVlODMzZmMwN2I4NCIsIlNUT1JFX1RZUEUiOiJWSUVUTkFNIiwiZXhwIjoyMDQzOTA3Mjk0LCJpc3MiOiJHb0Zvb2RCZXZlcmFnZS5BUEkiLCJhdWQiOiJHb0Zvb2RCZXZlcmFnZSJ9.2uEa2BtfGzBtVDc_chXjYnRxtbogN_JLkGfH2DLKMxg',
      Connection: 'keep-alive',
      'Content-Type': 'application/json',
      Origin: 'https://congcaphe.gofnb.gosell.vn',
      Referer: 'https://congcaphe.gofnb.gosell.vn/',
      'Sec-Fetch-Dest': 'empty',
      'Sec-Fetch-Mode': 'cors',
      'Sec-Fetch-Site': 'same-site',
      'User-Agent':
        'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/129.0.0.0 Safari/537.36 Edg/129.0.0.0',
      'X-STORE-ID': '824280ce-43e9-4235-2570-08dce101b936',
      'X-TIMEZONE-OFFSET': '-420',
      'platform-id': '6C626154-5065-616C-7466-6F7200000004',
      'sec-ch-ua': '"Microsoft Edge";v="129", "Not=A?Brand";v="8", "Chromium";v="129"',
      'sec-ch-ua-mobile': '?0',
      'sec-ch-ua-platform': '"Windows"',
    },
    data: data,
  };

  axios
    .request(config)
    .then(response => {
      console.log(JSON.stringify(response.data));
    })
    .catch(error => {
      console.log(error);
    });
}

// Stress test function
async function stressTest(concurrentRequests, iterations) {
  for (let i = 0; i < iterations; i++) {
    const requests = [];
    for (let j = 0; j < concurrentRequests; j++) {
      requests.push(sendRequest());
    }
    await Promise.all(requests);
    console.log(`Iteration ${i + 1} completed`);
  }
}

// Tham số: 10 yêu cầu đồng thời cho 100 lần lặp
stressTest(100, 100);

//sendRequest();
