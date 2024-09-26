// import { notification } from "antd";
// import React, { useCallback, useContext ,createContext} from "react";

// const ToastMessageType = {
//   SUCCESS: "success",
//   INFO: "info",
//   ERROR: "error",
//   WARNING: "warning",
// };

// const initContextState = {};
// const AppProviderContext = createContext(initContextState);
// export const useAppCtx = () => useContext(AppProviderContext);

// export const AppProvider = ({ children, ...props }) => {
//   const [messageApi, contextHolder] = notification.useNotification();

//   const Toast = {
//     show: (configs) => {
//       const {
//         messageType = ToastMessageType.SUCCESS |
//           ToastMessageType.INFO |
//           ToastMessageType.ERROR |
//           ToastMessageType.WARNING,
//         message,
//         icon = "",
//         className = "",
//         description = null,
//         closeIcon = false,
//         placement = "top" |
//           "topLeft" |
//           "topRight" |
//           "bottom" |
//           "bottomLeft" |
//           "bottomRight",
//         duration = 2,
//       } = configs;
//       messageApi[messageType]({
//         className: `toast-message theme-dark ${className}`,
//         closeIcon: closeIcon,
//         btn: "",
//         icon: icon,
//         message: message,
//         description: description,
//         duration: duration,
//         placement: placement,
//         maxCount: 1,
//         style: { fontFamily: props.fontFamily },
//       });
//     },
//     success: ({ message, description = null, placement = "top" }) => {
//       messageApi.success({
//         closeIcon: false,
//         btn: "",
//         message: message,
//         description: description,
//         duration: 2,
//         placement: placement,
//         maxCount: 1,
//       });
//     },
//     error: ({
//       message,
//       description = null,
//       placement = "top",
//       duration = 2,
//       className = "toast-message error",
//     }) => {
//       messageApi.error({
//         className: className,
//         closeIcon: false,
//         btn: "",
//         message: message,
//         description: description,
//         duration: duration,
//         placement: placement,
//         maxCount: 1,
//         style: { fontFamily: props.fontFamily },
//       });
//     },
//     info: () => {},
//     warning: ({ message, description, placement = "top", duration = 3 }) => {
//       messageApi["warning"]({
//         className: `toast-message theme-dark`,
//         message: message,
//         description: description,
//         duration: duration,
//         placement: placement,
//         maxCount: 1,
//       });
//     },
//   };

//   const getValue = useCallback(() => {
//     return {
//       messageApi,
//       Toast,
//     };
//   }, [messageApi, Toast]);

//   return (
//     <AppProviderContext.Provider value={getValue()}>
//       {contextHolder}
//       {children}
//     </AppProviderContext.Provider>
//   );
// };

