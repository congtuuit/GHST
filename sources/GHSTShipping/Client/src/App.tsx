import 'dayjs/locale/zh-cn';
import './main.css';
import { ConfigProvider, Spin, theme as antdTheme } from 'antd';
import enUS from 'antd/es/locale/en_US';
import viVN from 'antd/es/locale/vi_VN';
import dayjs from 'dayjs';
import localeVi from 'dayjs/locale/vi'; // Import Vietnamese locale
import { Suspense, useEffect } from 'react';
import { IntlProvider } from 'react-intl';
import { useDispatch, useSelector } from 'react-redux';
import { history, HistoryRouter } from '@/routes/history';
import { localeConfig, LocaleFormatter } from './locales';
import RenderRouter from './routes';
import { setGlobalState } from './stores/global.store';
import { rolesSelector } from './stores/user.store';

const App: React.FC = () => {
  const roles = useSelector(rolesSelector);

  const { locale } = useSelector(state => state.user);
  const { theme, loading } = useSelector(state => state.global);
  const dispatch = useDispatch();

  console.log("roles ", roles)

  const setTheme = (dark = false) => {
    dispatch(
      setGlobalState({
        theme: dark ? 'dark' : 'light',
      }),
    );
  };

  /** initial theme */
  useEffect(() => {
    setTheme(theme === 'light');

    // watch system theme change
    if (!localStorage.getItem('theme')) {
      const mql = window.matchMedia('(prefers-color-scheme: dark)');

      function matchMode(e: MediaQueryListEvent) {
        setTheme(e.matches);
      }

      mql.addEventListener('change', matchMode);
    }
  }, []);

  // set the locale for the user
  // more languages options can be added here
  useEffect(() => {
    if (locale === 'en_US') {
      dayjs.locale('en');
    } else if (locale === 'vi_VN') {
      dayjs.locale(localeVi);
    }
  }, [locale]);

  /**
   * handler function that passes locale
   * information to ConfigProvider for
   * setting language across text components
   */
  const getAntdLocale = () => {
    if (locale === 'en_US') {
      return enUS;
    } else if (locale === 'vi_VN') {
      return viVN;
    }
  };

  return (
    <ConfigProvider
      locale={getAntdLocale()}
      componentSize="middle"
      theme={{
        token: { 
          colorPrimary: '#c3002f',
          //colorPrimary: '#a50808',
         },
        //algorithm: theme === 'dark' ? antdTheme.darkAlgorithm : antdTheme.defaultAlgorithm,
        algorithm: antdTheme.defaultAlgorithm,
      }}
    >
      <IntlProvider locale={locale.split('_')[0]} messages={localeConfig[locale]}>
        <HistoryRouter history={history}>
          <Suspense fallback={null}>
            <Spin
              spinning={loading}
              className="app-loading-wrapper"
              style={{
                backgroundColor: theme === 'dark' ? 'rgba(0, 0, 0, 0.44)' : 'rgba(255, 255, 255, 0.44)',
              }}
              tip={<LocaleFormatter id="gloabal.tips.loading" />}
            ></Spin>
            <RenderRouter />
          </Suspense>
        </HistoryRouter>
      </IntlProvider>
    </ConfigProvider>
  );
};

export default App;
