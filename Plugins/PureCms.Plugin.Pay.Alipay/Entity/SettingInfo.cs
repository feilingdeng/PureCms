using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PureCms.Plugin.Pay.Alipay
{
    public class SettingInfo
    {
        private string _partner;//合作者身份ID
        private string _key;//交易安全检验码
        private string _seller;//收款支付宝帐户
        private string _privatekey;//私钥
        private decimal _payfee;//支付手续费
        private decimal _freemoney;//免费金额

        /// <summary>
        /// 合作者身份ID
        /// </summary>
        public string Partner
        {
            get { return _partner; }
            set { _partner = value; }
        }
        /// <summary>
        /// 交易安全检验码
        /// </summary>
        public string Key
        {
            get { return _key; }
            set { _key = value; }
        }
        /// <summary>
        /// 收款支付宝帐户
        /// </summary>
        public string Seller
        {
            get { return _seller; }
            set { _seller = value; }
        }
        /// <summary>
        /// 私钥
        /// </summary>
        public string PrivateKey
        {
            get { return _privatekey; }
            set { _privatekey = value; }
        }
        /// <summary>
        /// 支付手续费
        /// </summary>
        public decimal PayFee
        {
            get { return _payfee; }
            set { _payfee = value; }
        }
        /// <summary>
        /// 免费金额
        /// </summary>
        public decimal FreeMoney
        {
            get { return _freemoney; }
            set { _freemoney = value; }
        }
    }
}
