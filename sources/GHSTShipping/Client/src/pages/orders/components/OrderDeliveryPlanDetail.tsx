import NumberFormatter from '@/components/core/NumberFormatter';
import { IOrderDeiveryPricePlanDetailDto } from '@/interface/order/order.interface';
import React from 'react';

const OrderDeliveryPlanDetail: React.FC<{ data: IOrderDeiveryPricePlanDetailDto | undefined }> = ({ data }) => {
  if (!Boolean(data)) {
    return <></>;
  }

  const { name, minWeight, maxWeight, stepPrice, stepWeight, publicPrice } = data as IOrderDeiveryPricePlanDetailDto;

  return (
    <div>
      <hr />
      <div>
        <b>{name}</b>
      </div>
      <div>
        <NumberFormatter value={publicPrice} style="currency" currency="VND" locale="vi-VN" />
        <span style={{ paddingLeft: '5px' }}>
          (<NumberFormatter value={minWeight} style="decimal" /> - <NumberFormatter value={maxWeight} style="unit" unit="gram" />)
        </span>
      </div>
      <div>
        Mỗi <NumberFormatter value={stepWeight} style="unit" unit="gram" /> vượt quá <NumberFormatter value={maxWeight} style="unit" unit="gram" />
        <span style={{ paddingLeft: '5px' }}>
          +<NumberFormatter value={stepPrice} style="currency" currency="VND" locale="vi-VN" />
        </span>
      </div>
    </div>
  );
};

export default OrderDeliveryPlanDetail;
