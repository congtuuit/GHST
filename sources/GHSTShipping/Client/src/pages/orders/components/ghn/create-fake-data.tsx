export const createOrderFakeData1 = {
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
    {
      name: 'Áo Thun',
      weight: 100,
      quantity: 2,
      code: '0010',
    },
    {
      name: 'Áo Polo',
      weight: 150,
      quantity: 2,
      code: '0020',
    },
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
};

export const fakeOrder = {
  from_name: 'Hương Nguyễn', //Trường hợp nào nếu không truyền thông tin người gửi thì hệ thống sẽ mặc định lấy thông tin ở ShopID
  from_phone: '0395890960',
  from_address: '84A Trần Hữu Trang',
  from_ward_id: '21709',
  from_ward_name: 'Phường 10',
  from_district_id: 1457,
  from_district_name: 'Quận Phú Nhuận',
  from_province_id: 202,
  from_province_name: 'Hồ Chí Minh',

  to_name: 'Tú Văn', // Bắt buộc
  to_phone: '0974255412', // Bắt buộc
  to_address: '60A Trường Sơn', // Bắt buộc
  to_ward_id: '21411',
  to_ward_name: 'Phường 10', // Bắt buộc
  to_district_id: 1455,
  to_district_name: 'Quận Tân Bình', // Bắt buộc
  to_province_id: 202,
  to_province_name: 'Hồ Chí Minh',

  return_phone: '',
  return_address: '',
  return_district_name: '',
  return_ward_name: '',

  client_order_code: '', // hệ thống tự sinh
  cod_amount: 100000, // Tiền thu hộ cho người gửi. Maximum: 50000000, Giá trị mặc định: 0
  content: '', // Nội dung của đơn hàng. tối đa 2000 ký tự

  cod_failed_amount: 2000, //Thu thêm tiền khi giao hàng thất bại, Số cho phép nhập tự do

  weight: 50000, //Khối lượng của đơn hàng (gram). Tối đa : 50000 gram | Bắt buộc
  length: 200, // Tối đa : 200 cm | Bắt buộc
  width: 200, // Tối đa : 200 cm | Bắt buộc
  height: 200, // Tối đa : 200 cm | Bắt buộc

  pick_station_id: null, // Giá trị mặc định : null, Giá trị truyền vào > 0
  insurance_value: 0, // Giá trị mặc định: 0, Tối đa 5000000, Giá trị của đơn hàng ( Trường hợp mất hàng , bể hàng sẽ đền theo giá trị của đơn hàng).
  coupon: '', // Mã giảm giá.

  service_id: 0,

  // | Bắt buộc
  service_type_id: 2, //| Bắt buộc Mã loại dịch vụ: Gọi API lấy gói dịch vụ để lấy mã loại dịch vụ. https://api.ghn.vn/home/docs/detail?id=86, Trong đó:  2: Hàng nhẹ, 5: Hàng nặng
  payment_type_id: 2, //| Bắt buộc Mã người thanh toán phí dịch vụ. 1: Người bán/Người gửi. 2: Người mua/Người nhận.

  note: '', // Người gửi ghi chú cho tài xế. 5000 ký tự

  // | Bắt buộc
  required_note: '', // 500 ký tự, CHOTHUHANG | CHOXEMHANGKHONGTHU | KHONGCHOXEMHANG

  pick_shift: [1], // Array, Dùng để truyền ca lấy hàng , Sử dụng API Lấy danh sách ca lấy
  pickup_time: 0, // Truyền thời gian mong muốn lấy hàng , định dạng UnixtimeStamp.

  // | Bắt buộc
  items: [
    {
      name: 'Áo Polo', // | Bắt buộc
      code: '001',
      quantity: 1, // | Bắt buộc
      price: 0,
      length: 0,
      width: 0,
      height: 0,
      category: null,
    },
    {
      name: 'Áo Dài', // | Bắt buộc
      code: '002',
      quantity: 1, // | Bắt buộc
      price: 0,
      length: 0,
      width: 0,
      height: 0,
      category: null,
    }
  ],
};
