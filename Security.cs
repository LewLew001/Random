ValidateRequest="false"%>

using Microsoft.Security.Application;

string tmp = Sanitizer.GetSafeHtmlFragment(TextBox1.Text);

// param