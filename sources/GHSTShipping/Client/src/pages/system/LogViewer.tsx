import React, { useEffect, useState } from 'react';
import { clearLogFile, getLogFileContent, getLogFiles } from '@/features/logs';
import { Layout, Button, Table, Typography, message, Modal, Spin } from 'antd';
import { DeleteOutlined, FileTextOutlined } from '@ant-design/icons';
import NumberFormatter from '@/components/core/NumberFormatter';
import './style.css';

const { Header, Content } = Layout;
const { Title } = Typography;

interface LogFile {
  key: string;
  name: string;
  size: number;
}

const LogViewer: React.FC = () => {
  const [logFiles, setLogFiles] = useState<LogFile[]>([]);
  const [selectedFile, setSelectedFile] = useState<string | null>(null);
  const [logContent, setLogContent] = useState<string | null>(null);
  const [loading, setLoading] = useState<boolean>(false);
  const [isModalVisible, setIsModalVisible] = useState<boolean>(false);
  const [selectedRowKey, setSelectedRowKey] = useState<string | null>(null);

  // Fetch all log files on component mount
  useEffect(() => {
    const fetchLogFiles = async () => {
      setLoading(true);
      try {
        const response = await getLogFiles();
        const files = response.data.map((f: LogFile) => ({ key: f.name, name: f.name, size: f.size }));
        setLogFiles(files);
      } catch (error) {
        message.error('Error fetching log files');
      } finally {
        setLoading(false);
      }
    };

    fetchLogFiles();
  }, []);

  // Handle viewing content of a selected log file
  const handleFileClick = async (fileName: string) => {
    setLoading(true);
    try {
      const response = await getLogFileContent(fileName);
      setLogContent(response.data);
      setSelectedFile(fileName);
      setSelectedRowKey(fileName); // Set the selected row key
      setIsModalVisible(true); // Open the modal
    } catch (error) {
      message.error('Error fetching log file content');
    } finally {
      setLoading(false);
    }
  };

  // Handle clearing content of a log file
  const handleClearLog = async (fileName: string) => {
    Modal.confirm({
      title: 'Are you sure you want to clear this log?',
      content: `Log file: ${fileName}`,
      okText: 'Yes',
      okType: 'danger',
      cancelText: 'No',
      onOk: async () => {
        try {
          await clearLogFile(fileName);
          setLogFiles(prevFiles => prevFiles.filter(file => file.name !== fileName));
          message.success('Log file cleared successfully');
        } catch (error) {
          message.error('Error clearing log file');
        }
      },
    });
  };

  // Table columns configuration
  const columns = [
    {
      title: 'Log File Name',
      dataIndex: 'name',
      key: 'name',
      render: (text: string) => (
        <Button type="link" icon={<FileTextOutlined />} onClick={() => handleFileClick(text)}>
          {text}
        </Button>
      ),
    },
    {
      title: 'File Size',
      dataIndex: 'size',
      key: 'size',
      render: (text: number) => <NumberFormatter value={text} unit="byte" style="unit" />,
    },
    {
      title: 'Actions',
      key: 'actions',
      render: (_: any, record: LogFile) => (
        <Button type="primary" danger icon={<DeleteOutlined />} onClick={() => handleClearLog(record.name)}>
          Clear
        </Button>
      ),
    },
  ];

  // Close the modal
  const handleCloseModal = () => {
    setIsModalVisible(false);
    setLogContent(null);
    setSelectedFile(null);
  };

  return (
    <Layout style={{ minHeight: '100vh' }}>
      <Header style={{ backgroundColor: '#001529', padding: '0 20px' }}>
        <Title level={3} style={{ color: '#fff' }}>
          Log Viewer
        </Title>
      </Header>
      <Content style={{ margin: '20px' }}>
        <Spin spinning={loading}>
          <div style={{ backgroundColor: '#fff', padding: '20px', borderRadius: '8px' }}>
            <Title level={4}>Available Log Files</Title>
            <Table
              rowClassName={record => (record.name === selectedRowKey ? 'selected-row' : '')}
              onRow={record => ({
                onClick: () => setSelectedRowKey(record.name),
              })}
              columns={columns}
              dataSource={logFiles}
              pagination={false}
              rowKey="key"
              scroll={{ y: '73vh' }}
              style={{ marginBottom: '20px' }}
            />
          </div>
        </Spin>
      </Content>

      {/* Modal to show log content */}
      <Modal centered title={`Content of ${selectedFile}`} open={isModalVisible} onCancel={handleCloseModal} footer={null} width={'80%'}>
        <div
          style={{
            maxHeight: '80vh',
            overflowY: 'auto',
            backgroundColor: '#f5f5f5',
            borderRadius: '4px',
          }}
        >
          <pre style={{ whiteSpace: 'pre-wrap', margin: 0 }}>{logContent || 'No content available'}</pre>
        </div>
      </Modal>
    </Layout>
  );
};

export default LogViewer;
