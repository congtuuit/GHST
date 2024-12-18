import type { ColumnProps } from 'antd/es/table';

export interface TableColumnPropsOption {
  label: string;
  value: any;
}

type DatetimeType = 'hour' | 'minute' | 'second';
type TimeType = 'hour' | 'minute' | 'second';
type DateType = 'year' | 'month' | 'day';

type DatetimeFormatMap = {
  [x in DatetimeType]: string;
};

export const datetimeFormatMap: DatetimeFormatMap = {
  hour: 'YYYY-MM-DD HH',
  minute: 'YYYY-MM-DD HH:mm',
  second: 'YYYY-MM-DD HH:mm:ss',
};

export const revertdatetimeFormatMap: DatetimeFormatMap = {
  hour: 'DD/MM/YYYY HH',
  minute: 'DD/MM/YYYY HH:mm',
  second: 'DD/MM/YYYY HH:mm:ss',
};

type DateFormatMap = {
  [x in DateType]: string;
};

export const dateFormatMap: DateFormatMap = {
  year: 'YYYY',
  month: 'YYYY-MM',
  day: 'YYYY-MM-DD',
};

export const revertDateFormatMap: DateFormatMap = {
  year: 'YYYY',
  month: 'MM/YYYY',
  day: 'DD/MM/YYYY',
};

type TimeFormatMap = {
  [x in TimeType]: string;
};

export const timeFormatMap: TimeFormatMap = {
  hour: 'HH',
  minute: 'HH:mm',
  second: 'HH:mm:ss',
};

export interface MyTableColumnProps<T> extends ColumnProps<T> {
  options?: TableColumnPropsOption[];
  datetime?: DatetimeType | boolean;
  date?: DateType | boolean;
  time?: TimeType | boolean;
}
