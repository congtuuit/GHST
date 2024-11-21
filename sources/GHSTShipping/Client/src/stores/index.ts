import { configureStore } from '@reduxjs/toolkit';
import { persistStore, persistReducer } from 'redux-persist';
import storage from 'redux-persist/lib/storage'; // Sử dụng localStorage
import rootReducer from './rootReducer';

// Cấu hình redux-persist
const persistConfig = {
  key: 'root', // Tên key trong localStorage
  storage, // Sử dụng localStorage cho việc lưu trữ
  whitelist: ['user', 'global', 'order'], // Danh sách reducer cần lưu trữ
};

const persistedReducer = persistReducer(persistConfig, rootReducer);

// Tạo Redux store với persistedReducer
const store = configureStore({
  reducer: persistedReducer,
});

const persistor = persistStore(store);

// Xuất các kiểu dữ liệu và đối tượng cần thiết
export type AppState = ReturnType<typeof rootReducer>;
export type AppDispatch = typeof store.dispatch;
export type AppStore = typeof store;

export { persistor };
export default store;
