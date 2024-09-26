import { history, useIntl } from "@umijs/max";
import { Button, Result } from "antd";
import React from "react";

const NoFoundPage = () => (
  <Result
    status="404"
    title="404"
    subTitle={"Không tìm thấy trang"}
    extra={
      <Button type="primary" onClick={() => history.push("/")}>
        Quay lại trang chủ
      </Button>
    }
  />
);

export default NoFoundPage;
