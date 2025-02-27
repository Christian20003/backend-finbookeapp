namespace FinBookeAPI.Services.Authentication;

public static class Email
{
    public static string GetCodeBody(string code)
    {
        return @"
        <!DOCTYPE html>
        <html lang='en'>
        <head>
            <meta charset='UTF-8'>
            <meta name='viewport' content='width=device-width, initial-scale=1.0'>
            <title>Security Code Email</title>
            <style>
                body {
                    font-family: Arial, sans-serif;
                    background-color: #f4f4f4;
                    margin: 0;
                    padding: 20px;
                }
                .container {
                    max-width: 600px;
                    margin: auto;
                    background: white;
                    border-radius: 8px;
                    box-shadow: 0 2px 10px rgba(0, 0, 0, 0.1);
                    padding: 20px;
                }
                h1 {
                    color: #333;
                    text-align: center;
                }
                .code-box {
                    display: flex;
                    justify-content: center;
                    align-items: center;
                    background-color: #e9ecef;
                    border: 2px solid #007bff;
                    border-radius: 8px;
                    font-size: 24px;
                    font-weight: bold;
                    height: 60px;
                    width: 100px;
                    margin: 20px auto;
                }
                .footer {
                    text-align: center;
                    font-size: 12px;
                    color: #777;
                    margin-top: 20px;
                }
            </style>
        </head>
        <body>
            <div class='container'>
                <h1>Your Security Code</h1>
                <p>Dear User,</p>
                <p>Thank you for your request. Below is your security code:</p>
                <div class='code-box'>"
            + code
            + @"</div>
                <p>Please enter this code to complete the verification process.</p>
                <div class='footer'>
                    &copy; 2025 FinBooKe. All rights reserved.
                </div>
            </div>
        </body>
        </html>
        ";
    }

    public static string GetPasswordEmail(string password)
    {
        return @"
        <!DOCTYPE html>
        <html lang='en'>
        <head>
            <meta charset='UTF-8'>
            <meta name='viewport' content='width=device-width, initial-scale=1.0'>
            <title>Security Code Email</title>
            <style>
                body {
                    font-family: Arial, sans-serif;
                    background-color: #f4f4f4;
                    margin: 0;
                    padding: 20px;
                }
                .container {
                    max-width: 600px;
                    margin: auto;
                    background: white;
                    border-radius: 8px;
                    box-shadow: 0 2px 10px rgba(0, 0, 0, 0.1);
                    padding: 20px;
                }
                h1 {
                    color: #333;
                    text-align: center;
                }
                .code-box {
                    display: flex;
                    justify-content: center;
                    align-items: center;
                    background-color: #e9ecef;
                    border: 2px solid #007bff;
                    border-radius: 8px;
                    font-size: 24px;
                    font-weight: bold;
                    height: 60px;
                    width: 100px;
                    margin: 20px auto;
                }
                .footer {
                    text-align: center;
                    font-size: 12px;
                    color: #777;
                    margin-top: 20px;
                }
            </style>
        </head>
        <body>
            <div class='container'>
                <h1>Your New Password</h1>
                <p>Dear User,</p>
                <p>Thank you for your request. Below is your new password:</p>
                <div class='code-box'>"
            + password
            + @"</div>
                <p>Please use this new password in the login screen and change it immediatly with a new password.</p>
                <div class='footer'>
                    &copy; 2025 FinBooKe. All rights reserved.
                </div>
            </div>
        </body>
        </html>
        ";
    }
}
