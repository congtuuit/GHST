import { Button, DatePicker, Popover, Space } from 'antd';
import React, { useState, CSSProperties, useEffect } from 'react';
import OrderFilter, { OrderFilterState } from './ghn/OrderFilter';
import { DownOutlined } from '@ant-design/icons';
import { useDispatch } from 'react-redux';
import { setOrderFilter } from '@/features/order/orderSlice';
import dayjs, { Dayjs } from 'dayjs';

interface AdminOrderFilterWrapperProps {
  style?: CSSProperties;
  styleContent?: CSSProperties;
}

const AdminOrderFilterWrapper: React.FC<AdminOrderFilterWrapperProps> = (props: AdminOrderFilterWrapperProps) => {
  const { style, styleContent } = props;
  const [open, setOpen] = useState(false);
  const dispatch = useDispatch();
  const [fromDate, setFromDate] = useState<Dayjs | null>(dayjs().subtract(1, 'month').startOf('month'));
  const [toDate, setToDate] = useState<Dayjs | null>(dayjs());

  const handleOpenChange = (visible: boolean) => {
    setOpen(visible);
  };

  const handleChangeFromDate = (date: Dayjs | null) => {
    setFromDate(date);
  };

  const handleChangeToDate = (date: Dayjs | null) => {
    setToDate(date);
  };

  const onFilterChange = (filter: OrderFilterState) => {
    dispatch(
      setOrderFilter({
        ...filter,
      }),
    );
  };

  useEffect(() => {
    dispatch(
      setOrderFilter({
        fromDate: fromDate,
        toDate: toDate,
      }),
    );
  }, [fromDate, toDate]);

  return (
    <Space direction="horizontal" style={style}>
      <Popover
        content={<OrderFilter onFilterChange={onFilterChange} style={styleContent} />}
        trigger="click"
        open={open}
        onOpenChange={handleOpenChange}
      >
        <Button type="text" onClick={() => setOpen(!open)}>
          Lọc hiển thị <DownOutlined />
        </Button>
      </Popover>
      <div>
        Từ <DatePicker value={fromDate} onChange={handleChangeFromDate} placeholder="Chọn ngày" format="DD/MM/YYYY" />
      </div>
      <div>
        Đến <DatePicker value={toDate} onChange={handleChangeToDate} placeholder="Chọn ngày" format="DD/MM/YYYY" />
      </div>
    </Space>
  );
};

export default AdminOrderFilterWrapper;
