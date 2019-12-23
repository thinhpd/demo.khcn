
<?xml version="1.0" encoding="UTF-8" ?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
	<xsl:output method="html" doctype-system="http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd"
		doctype-public="-//W3C//DTD XHTML 1.0 Transitional//EN" />
	<xsl:variable name="title" select="/rss/channel/title" />
	<xsl:variable name="feedUrl" select="/rss/channel/link" />
	<xsl:template match="/">
		<xsl:element name="html">
			<head>
				<title>
					<xsl:value-of select="$title" />
				</title>
				<link rel="alternate" type="application/rss+xml" title="RSS" href="{$feedUrl}" />
				<link rel="stylesheet" type="text/css" href="/Content/Publishing/rss/rss.css" />
			</head>
			<xsl:apply-templates select="rss/channel" />
		</xsl:element>
	</xsl:template>
	<xsl:template match="channel">
		<body>
			<table border="0" width="780" cellspacing="0" cellpadding="0" bgcolor="#FFFFFF">
				<tr valign="top">
					<td style="padding-left:14px">
						<div class="bigtitle">
							<h1>
								<a href="{generator}">
									<xsl:value-of select="$title" />
								</a>
							</h1>
							<b>Đăng kí kênh RSS này</b>
							<div class="space"></div>
							<div class="space2"></div>
							<a href="http://us.rd.yahoo.com/my/*http://add.my.yahoo.com/rss?url={$feedUrl}"
								onMouseover="window.status='http://us.rd.yahoo.com/my/*http://add.my.yahoo.com/rss?url={$feedUrl}'; return true;"
								onMouseout="window.status=' '; return true;">
								<img border="0" src="/images/Rss/addtomyyahoo4.gif" width="91" height="17" alt="Add to My Yahoo" />
							</a>
							<div class="space2"></div>
							<a href="http://www.newsgator.com/ngs/subscriber/subext.aspx?url={$feedUrl}" onMouseover="window.status='http://www.newsgator.com/ngs/subscriber/subext.aspx?url={$feedUrl}'; return true;"
								onMouseout="window.status=' '; return true;">
								<img src="/images/Rss/newsgator.gif" width="91" height="17" alt="Subscribe in NewsGator Online"
									border="0" />
							</a>
							<div class="space2"></div>
							<a href="http://client.pluck.com/pluckit/prompt.aspx?a={$feedUrl}" onMouseover="window.status='http://client.pluck.com/pluckit/prompt.aspx?a={$feedUrl}'; return true;"
								onMouseout="window.status=' '; return true;">
								<img border="0" src="/images/Rss/pluckit.png" width="80" height="15" alt="Add to Pluck" />
							</a>
							<div class="space2"></div>
							<a href="http://www.bloglines.com/sub/{$feedUrl}" onMouseover="window.status='http://www.bloglines.com/sub/{$feedUrl}'; return true;"
								onMouseout="window.status=' '; return true;">
								<img src="/images/Rss/bloglines.gif" width="80" height="15" alt="Subscribe in Bloglines"
									border="0" />
							</a>
							<div class="space2"></div>
							<a href="http://www.rojo.com/add-subscription?resource={$feedUrl}" onMouseover="window.status='http://www.rojo.com/add-subscription?resource={$feedUrl}'; return true;"
								onMouseout="window.status=' '; return true;">
								<img src="/images/Rss/add-to-rojo.gif" width="52" height="17" alt="Subscribe in Rojo"
									border="0" />
							</a>
							<div class="space2"></div>
						</div>
					</td>
				</tr>
				<tr>
					<td style="padding:14px">
						<xsl:apply-templates select="item" />
					</td>
				</tr>
			</table>
		</body>
	</xsl:template>
	<xsl:template match="item">
		<div class="story">
			<div class="title">
				<a href="{link}">
					<xsl:value-of select="title" />
				</a>
			</div>
			<div class="summary">
				<xsl:value-of select="description" />
			</div>
		</div>
	</xsl:template>
</xsl:stylesheet>