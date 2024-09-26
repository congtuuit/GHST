import { LockOutlined, UserOutlined } from "@ant-design/icons";
import {
  LoginFormPage,
  ProConfigProvider,
  ProFormText,
} from "@ant-design/pro-components";
import { theme } from "antd";
import { useState } from "react";
import { useNavigate } from "react-router-dom";

const Page = () => {
  const navigate = useNavigate();
  const { token } = theme.useToken();
  const [isLoading, setIsLoading] = useState(false);
  const [formMode, setFormMode] = useState("login"); //forgot_password | login

  const handleLogin = (values) => {
    setIsLoading(true);

    if (formMode === "forgot_password") {
      //   authServices.resetPasswordAsync(values, (success) => {
      //     setIsLoading(false);
      //     if (success) {
      //       Toast.success({
      //         message: "Đã gửi mật khẩu mới qua địa chỉ email của bạn.",
      //       });
      //       setFormMode("login");
      //     } else {
      //       Toast.error({
      //         message: "Thông tin yêu cầu cấp lại mật khẩu không đúng!",
      //       });
      //     }
      //   });
    } else {
      //   authServices.loginAsync(values, (success) => {
      //     setIsLoading(false);
      //     if (success) {
      //       navigate("/");
      //     } else {
      //       Toast.error({ message: "Tài khoản hoặc mật khẩu không đúng!" });
      //     }
      //   });
    }
  };

  return (
    <div
      className="login-page"
      style={{
        height: "100vh",
      }}
    >
      <LoginFormPage
        loading={isLoading}
        className="login-form"
        onFinish={handleLogin}
        backgroundImageUrl="assets/fmt.webp"
        backgroundVideoUrl="assets/jXRBRK_VAwoAAAAAAAAAAAAAK4eUAQBr.mp4"
        title={formMode === "forgot_password" ? "Quên mật khẩu" : "Đăng nhập"}
        containerStyle={{
          backgroundColor: "rgba(0, 0, 0,0.65)",
          backdropFilter: "blur(4px)",
        }}
        submitter={{
          searchConfig: {
            submitText:
              formMode === "forgot_password" ? "Cấp lại mật khẩu" : "Đăng nhập",
          },
        }}
      >
        <div
          style={{
            height: 20,
          }}
        ></div>

        {formMode === "login" && (
          <>
            <ProFormText
              name="username"
              fieldProps={{
                size: "large",
                prefix: (
                  <UserOutlined
                    style={{
                      color: token.colorText,
                    }}
                    className={"prefixIcon"}
                  />
                ),
              }}
              placeholder={"Tài khoản"}
              rules={[
                {
                  required: true,
                  message: "Vui lòng nhập tài khoản",
                },
              ]}
            />
            <ProFormText.Password
              name="password"
              fieldProps={{
                size: "large",
                prefix: (
                  <LockOutlined
                    style={{
                      color: token.colorText,
                    }}
                    className={"prefixIcon"}
                  />
                ),
              }}
              placeholder={"Mật khẩu"}
              rules={[
                {
                  required: true,
                  message: "Vui lòng nhập mật khẩu !",
                },
              ]}
            />

            <a
              style={{
                float: "right",
                marginBottom: "15px",
              }}
              onClick={() => setFormMode("forgot_password")}
            >
              Quên mật khẩu
            </a>
          </>
        )}

        {formMode === "forgot_password" && (
          <>
            <ProFormText
              name="username"
              fieldProps={{
                size: "large",
                prefix: (
                  <UserOutlined
                    style={{
                      color: token.colorText,
                    }}
                    className={"prefixIcon"}
                  />
                ),
              }}
              placeholder={"Tài khoản"}
              rules={[
                {
                  required: true,
                  message: "Vui lòng nhập tài khoản",
                },
              ]}
            />
            <ProFormText
              name="email"
              fieldProps={{
                size: "large",
                prefix: (
                  <UserOutlined
                    style={{
                      color: token.colorText,
                    }}
                    className={"prefixIcon"}
                  />
                ),
              }}
              placeholder={"Địa chỉ email"}
              rules={[
                {
                  required: true,
                  message: "Vui lòng nhập địa chỉ email",
                },
              ]}
            />
            <a
              style={{
                float: "right",
                marginBottom: "15px",
              }}
              onClick={() => setFormMode("login")}
            >
              Quay lại đăng nhập
            </a>
          </>
        )}
      </LoginFormPage>
    </div>
  );
};

export default () => {
  return (
    <ProConfigProvider dark>
      <Page />
    </ProConfigProvider>
  );
};
