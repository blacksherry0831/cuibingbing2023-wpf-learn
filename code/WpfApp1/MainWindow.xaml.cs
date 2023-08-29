using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Packets;
using MQTTnet.Protocol;
using MQTTnet.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp1
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();


            var t=InitServerAsync();

            t.Wait();

            initClient();


           


        }

        [Obsolete]
        private async Task CuiPublishAsync() {


            char[] payload = null;
            string topic = null;
            var message = new MqttApplicationMessage()
            {

                Topic = topic,
                Payload = Encoding.UTF8.GetBytes(payload),
                QualityOfServiceLevel = MqttQualityOfServiceLevel.ExactlyOnce,
               
            };
            await _MqttClient.PublishAsync(message);




        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("hello");

        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
                    this.tb1.Text = this.slider0.Value.ToString();
                    this.tb2.Text = this.slider0.Value.ToString();
                    this.tb3.Text = this.slider0.Value.ToString();
        }

        private void tb1_TextChanged(object sender, TextChangedEventArgs e)
        {

            bool success = double.TryParse(tb1.Text, out double result);
            
            if (success) {
                this.slider0.Value = result; 
            }

        }

        private void tb2_TextChanged(object sender, TextChangedEventArgs e)
        {
            bool success = double.TryParse(tb2.Text,out double result);
            if (success) {
                this.slider0.Value = result;
                    }

        }

        private void tb3_TextChanged(object sender, TextChangedEventArgs e)
        {
            bool success = double.TryParse(tb3.Text, out double result);
            if (success) {
                this.slider0.Value = result;
            }

        }


        private MqttServer _MqttServer;
        private MQTTnet.Client.IMqttClient _MqttClient;
        string ip = "127.0.0.1";
        int port = 10001;
        public string topic = "XXXXXXXXXXXXXX";
        bool withPersistentSessions = true;

        public async Task InitServerAsync()
        {

          
            

            MqttServerOptionsBuilder mqttServerOptionsBuilder = new MqttServerOptionsBuilder();  // MQTT服务器配置
            mqttServerOptionsBuilder.WithDefaultEndpoint();
            mqttServerOptionsBuilder.WithDefaultEndpointBoundIPAddress(IPAddress.Parse(ip));  // 设置Server的IP
            mqttServerOptionsBuilder.WithDefaultEndpointPort(port);                           // 设置Server的端口号
                                                                                              //mqttServerOptionsBuilder.WithEncryptedEndpointPort(port);                        // 使用加密的端点端口
            mqttServerOptionsBuilder.WithPersistentSessions(withPersistentSessions);  // 持续会话
            mqttServerOptionsBuilder.WithConnectionBacklog(2000);                     // 最大连接数
            

            MqttServerOptions mqttServerOptions = mqttServerOptionsBuilder.Build();
            _MqttServer = new MqttFactory().CreateMqttServer(mqttServerOptions);  // 创建服务（配置）
            _MqttServer.ApplicationMessageNotConsumedAsync += ApplicationMessageNotConsumedHandle;  // 设置消息处理程序
            _MqttServer.ClientSubscribedTopicAsync += MqttServer_ClientSubscribedTopicAsync;


            await _MqttServer.StartAsync();  // 开启服务


           

            /// <summary>
            /// 设置消息处理程序
            /// </summary>
           


        }

        private Task MqttServer_ClientSubscribedTopicAsync(ClientSubscribedTopicEventArgs arg)
        {
            Console.WriteLine("服务器>> 客户端订阅" + arg.ClientId);
            return Task.CompletedTask;
        }

        private Task ApplicationMessageNotConsumedHandle(ApplicationMessageNotConsumedEventArgs arg)
        {
            var msg = arg.ApplicationMessage.PayloadSegment;

            
            Console.WriteLine("服务器>> 客户端订阅" + "ApplicationMessageNotConsumedHandle");

            Console.WriteLine(Encoding.UTF8.GetString(msg.Array));

            return Task.CompletedTask;
        }

        

       public async void initClient() {


           

            MqttClientOptionsBuilder mqttClientOptionsBuilder = new MqttClientOptionsBuilder();
            mqttClientOptionsBuilder.WithTcpServer(ip, port);          // 设置MQTT服务器地址
           
            mqttClientOptionsBuilder.WithClientId("client 00");  // 设置客户端序列号
            MqttClientOptions options = mqttClientOptionsBuilder.Build();

            _MqttClient = new MqttFactory().CreateMqttClient();
          
            _MqttClient.ApplicationMessageReceivedAsync += ApplicationMessageReceivedHandle;  // 发送消息事件

         

            try {

               var x=await _MqttClient.ConnectAsync(options);  // 连接

                Console.WriteLine(x.ResultCode);

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());

            }

          


            MqttTopicFilter topicFilter = new MqttTopicFilterBuilder().WithTopic(topic).Build();
            await _MqttClient.SubscribeAsync(topicFilter, CancellationToken.None);  // 订阅



            var applicationMessage = new MqttApplicationMessageBuilder()
                .WithTopic(topic)
                .WithPayload("19.5")
                .Build();

            await _MqttClient.PublishAsync(applicationMessage, CancellationToken.None);

            Console.WriteLine("PublishAsync"+ "19.5");


        }

        private Task ApplicationMessageReceivedHandle(MqttApplicationMessageReceivedEventArgs arg)
        {
            Console.WriteLine("客户端：ApplicationMessageReceivedHandle");


            Console.WriteLine("Received application message.");
            var arr = arg.ApplicationMessage.PayloadSegment.Array;
            Console.WriteLine(Encoding.UTF8.GetString(arr));

            return Task.CompletedTask;
        }


    }



}
